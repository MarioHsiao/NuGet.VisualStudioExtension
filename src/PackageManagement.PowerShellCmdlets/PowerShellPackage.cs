﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

extern alias Legacy;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using NuGet.Versioning;
using SemanticVersion = Legacy::NuGet.SemanticVersion;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Represent the view of packages by Id and Versions
    /// </summary>
    public class PowerShellPackage
    {
        public string Id { get; set; }

        public IEnumerable<NuGetVersion> Versions
        {
            get
            {
                return ThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    var result = (await AsyncLazyVersions.GetValueAsync()) ?? Enumerable.Empty<NuGetVersion>();

                    if (result.Any())
                    {
                        if (AllVersions)
                        {
                            return result;
                        }
                        else
                        {
                            // result has at least 1 element, so call First()
                            var nVersion = result.First();
                            return new[] { nVersion };
                        }
                    }

                    return null;
                });
            }
        }

        public AsyncLazy<IEnumerable<NuGetVersion>> AsyncLazyVersions { get; set; }

        public SemanticVersion Version
        {
            get
            {
                var nVersion = Versions.FirstOrDefault();

                if (nVersion != null)
                {
                    SemanticVersion sVersion;
                    SemanticVersion.TryParse(nVersion.ToString(), out sVersion);
                    return sVersion;
                }

                return null;
            }
        }

        public bool AllVersions { get; set; }
    }
}
