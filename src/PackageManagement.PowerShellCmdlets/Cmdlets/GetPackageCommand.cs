﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Threading;
using NuGet.Packaging;
using NuGet.ProjectManagement;
using NuGet.Protocol.VisualStudio;
using NuGet.Versioning;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// This command lists the available packages which are either from a package source or installed in the
    /// current solution.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Package", DefaultParameterSetName = ParameterAttribute.AllParameterSets)]
    [OutputType(typeof(IPowerShellPackage))]
    public class GetPackageCommand : NuGetPowerShellBaseCommand
    {
        private const int DefaultFirstValue = 50;
        private bool _enablePaging;

        [Parameter(Position = 0)]
        [ValidateNotNullOrEmpty]
        public string Filter { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Project")]
        [ValidateNotNullOrEmpty]
        public string ProjectName { get; set; }

        [Parameter(ParameterSetName = "Remote")]
        [Parameter(ParameterSetName = "Updates")]
        [ValidateNotNullOrEmpty]
        public string Source { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Remote")]
        [Alias("Online", "Remote")]
        public SwitchParameter ListAvailable { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Updates")]
        public SwitchParameter Updates { get; set; }

        [Parameter(ParameterSetName = "Remote")]
        [Parameter(ParameterSetName = "Updates")]
        public SwitchParameter AllVersions { get; set; }

        [Parameter(ParameterSetName = "Remote")]
        public int PageSize { get; set; }

        [Parameter]
        [Alias("Prerelease")]
        public SwitchParameter IncludePrerelease { get; set; }

        [Parameter]
        [ValidateRange(0, Int32.MaxValue)]
        public virtual int First { get; set; }

        [Parameter]
        [ValidateRange(0, Int32.MaxValue)]
        public int Skip { get; set; }

        /// <summary>
        /// Determines if local repository are not needed to process this command
        /// </summary>
        protected bool UseRemoteSourceOnly { get; set; }

        /// <summary>
        /// Determines if a remote repository will be used to process this command.
        /// </summary>
        protected bool UseRemoteSource { get; set; }

        protected virtual bool CollapseVersions { get; set; }

        public List<NuGetProject> Projects { get; private set; }

        private void Preprocess()
        {
            UseRemoteSourceOnly = ListAvailable.IsPresent || (!String.IsNullOrEmpty(Source) && !Updates.IsPresent);
            UseRemoteSource = ListAvailable.IsPresent || Updates.IsPresent || !String.IsNullOrEmpty(Source);
            CollapseVersions = !AllVersions.IsPresent;
            UpdateActiveSourceRepository(Source);
            GetNuGetProject(ProjectName);

            // When ProjectName is not specified, get all of the projects in the solution
            if (string.IsNullOrEmpty(ProjectName))
            {
                Projects = VsSolutionManager.GetNuGetProjects().ToList();
            }
            else
            {
                Projects = new List<NuGetProject> { Project };
            }
        }

        protected override void ProcessRecordCore()
        {
            Preprocess();

            // If Remote & Updates set of parameters are not specified, list the installed package.
            if (!UseRemoteSource)
            {
                CheckForSolutionOpen();

                Dictionary<NuGetProject, IEnumerable<PackageReference>> packagesToDisplay = GetInstalledPackages(Projects, Filter, Skip, First).Result;
                WriteInstalledPackages(packagesToDisplay);
            }
            else
            {
                if (PageSize != 0)
                {
                    _enablePaging = true;
                    First = PageSize;
                }
                else if (First == 0)
                {
                    First = DefaultFirstValue;
                }

                if (Filter == null)
                {
                    Filter = string.Empty;
                }

                // Find avaiable packages from the current source and not taking targetframeworks into account. 
                if (UseRemoteSourceOnly)
                {
                    IEnumerable<PSSearchMetadata> remotePackages = GetPackagesFromRemoteSource(Filter, Enumerable.Empty<string>(), IncludePrerelease.IsPresent, Skip, First);
                    WritePackagesFromRemoteSource(remotePackages, true);

                    if (_enablePaging)
                    {
                        WriteMoreRemotePackagesWithPaging(remotePackages);
                    }
                }
                // Get package udpates from the current source and taking targetframeworks into account.
                else
                {
                    CheckForSolutionOpen();

                    foreach (NuGetProject project in Projects)
                    {
                        IEnumerable<string> frameworks = PowerShellCmdletsUtility.GetProjectTargetFrameworks(project);
                        IEnumerable<PackageReference> installedPackages = project.GetInstalledPackagesAsync(CancellationToken.None).Result;
                        Dictionary<PSSearchMetadata, NuGetVersion> remoteUpdates = GetPackageUpdatesFromRemoteSource(installedPackages, frameworks, IncludePrerelease.IsPresent, Skip, First);
                        WriteUpdatePackagesFromRemoteSource(remoteUpdates, project);
                    }
                }
            }
        }

        /// <summary>
        /// Output installed packages to the project(s)
        /// </summary>
        /// <param name="dictionary"></param>
        private void WriteInstalledPackages(Dictionary<NuGetProject, IEnumerable<PackageReference>> dictionary)
        {
            // Get the PowerShellPackageWithProjectView
            var view = PowerShellInstalledPackage.GetPowerShellPackageView(dictionary);
            if (view.Any())
            {
                WriteObject(view, enumerateCollection: true);
            }
            else
            {
                LogCore(MessageLevel.Info, Resources.Cmdlet_NoPackagesInstalled);
            }
        }

        /// <summary>
        /// Output packages found from the current remote source
        /// </summary>
        /// <param name="packages"></param>
        /// <param name="outputWarning"></param>
        private void WritePackagesFromRemoteSource(IEnumerable<PSSearchMetadata> packages, bool outputWarning = false)
        {
            // Write warning message for Get-Package -ListAvaialble -Filter being obsolete
            // and will be replaced by Find-Package [-Id] 
            VersionType versionType;
            string message;
            if (CollapseVersions)
            {
                versionType = VersionType.latest;
                message = "Find-Package [-Id]";
            }
            else
            {
                versionType = VersionType.all;
                message = "Find-Package [-Id] -AllVersions";
            }

            // Output list of PowerShellPackages
            if (outputWarning && !string.IsNullOrEmpty(Filter))
            {
                LogCore(MessageLevel.Warning, string.Format(CultureInfo.CurrentCulture, Resources.Cmdlet_CommandObsolete, message));
            }

            WritePackages(packages, versionType);
        }

        /// <summary>
        /// Output packages found from the current remote source with specified page size
        /// e.g. Get-Package -ListAvailable -PageSize 20
        /// </summary>
        /// <param name="packagesToDisplay"></param>
        private void WriteMoreRemotePackagesWithPaging(IEnumerable<PSSearchMetadata> packagesToDisplay)
        {
            // Display more packages with paging
            int pageNumber = 1;
            while (true)
            {
                packagesToDisplay = GetPackagesFromRemoteSource(Filter, Enumerable.Empty<string>(), IncludePrerelease.IsPresent, pageNumber * PageSize, PageSize);
                if (packagesToDisplay.Count() != 0)
                {
                    // Prompt to user and if want to continue displaying more packages
                    int command = AskToContinueDisplayPackages();
                    if (command == 0)
                    {
                        // If yes, display the next page of (PageSize) packages
                        WritePackagesFromRemoteSource(packagesToDisplay);
                    }
                    else
                    {
                        break;
                    }
                }
                pageNumber++;
            }
        }

        /// <summary>
        /// Output package updates to current project(s) found from the current remote source
        /// </summary>
        /// <param name="packagesToDisplay"></param>
        private void WriteUpdatePackagesFromRemoteSource(Dictionary<PSSearchMetadata, NuGetVersion> packagesToDisplay, NuGetProject project)
        {
            VersionType versionType;
            if (CollapseVersions)
            {
                versionType = VersionType.latest;
            }
            else
            {
                versionType = VersionType.updates;
            }

            WritePackages(packagesToDisplay, versionType, project);
        }

        private void WritePackages(IEnumerable<PSSearchMetadata> packages, VersionType versionType)
        {
            var view = PowerShellRemotePackage.GetPowerShellPackageView(packages, versionType);

            if (view.Any())
            {
                WriteObject(view, enumerateCollection: true);
            }
            else
            {
                LogCore(MessageLevel.Info, Resources.Cmdlet_GetPackageNoPackageFound);
            }
        }

        private void WritePackages(Dictionary<PSSearchMetadata, NuGetVersion> remoteUpdates, VersionType versionType, NuGetProject project)
        {
            List<PowerShellUpdatePackage> view = new List<PowerShellUpdatePackage>();
            foreach (KeyValuePair<PSSearchMetadata, NuGetVersion> pair in remoteUpdates)
            {
                PowerShellUpdatePackage package =
                    PowerShellUpdatePackage.GetPowerShellPackageUpdateView(pair.Key, pair.Value, versionType, project);

                // Question for CR - Is it ok just to add the package since it must have a version?
                view.Add(package);
            }

            if (view.Any())
            {
                WriteObject(view, enumerateCollection: true);
            }
            else
            {
                LogCore(MessageLevel.Info, string.Format(CultureInfo.CurrentCulture, Resources.Cmdlet_NoPackageUpdates, project.GetMetadata<string>(NuGetProjectMetadataKeys.Name)));
            }
        }

        private int AskToContinueDisplayPackages()
        {
            // Add a line before message prompt
            WriteLine();
            var choices = new Collection<ChoiceDescription>
                {
                    new ChoiceDescription(Resources.Cmdlet_Yes, Resources.Cmdlet_DisplayMorePackagesYesHelp),
                    new ChoiceDescription(Resources.Cmdlet_No, Resources.Cmdlet_DisplayMorePackagesNoHelp)
                };

            int choice = Host.UI.PromptForChoice(string.Empty, Resources.Cmdlet_PrompToDisplayMorePackages, choices, defaultChoice: 1);

            Debug.Assert(choice >= 0 && choice < 2);
            // Add a line after
            WriteLine();
            return choice;
        }
    }
}
