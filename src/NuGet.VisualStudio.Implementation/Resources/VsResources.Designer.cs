﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NuGet.VisualStudio.Implementation.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class VsResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal VsResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NuGet.VisualStudio.Implementation.Resources.VsResources", typeof(VsResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Package list contains invalid or duplicate entries..
        /// </summary>
        internal static string InvalidPackageList {
            get {
                return ResourceManager.GetString("InvalidPackageList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The provided version string is not a valid semantic version..
        /// </summary>
        internal static string InvalidSemanticVersionString {
            get {
                return ResourceManager.GetString("InvalidSemanticVersionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error accessing Extension Manager..
        /// </summary>
        internal static string PreinstalledPackages_ExtensionManagerError {
            get {
                return ResourceManager.GetString("PreinstalledPackages_ExtensionManagerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not add all required packages to the project. The following packages failed to install from &apos;{0}&apos;:.
        /// </summary>
        internal static string PreinstalledPackages_FailedToInstallPackage {
            get {
                return ResourceManager.GetString("PreinstalledPackages_FailedToInstallPackage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The preinstalled packages have a reference to a missing Extension. Could not find an Extension with ID &apos;{0}&apos;..
        /// </summary>
        internal static string PreinstalledPackages_InvalidExtensionId {
            get {
                return ResourceManager.GetString("PreinstalledPackages_InvalidExtensionId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The preinstalled packages have a reference to a missing Registry value. Could not find a Registry key with name &apos;{0}&apos; under &apos;{1}&apos;..
        /// </summary>
        internal static string PreinstalledPackages_InvalidRegistryValue {
            get {
                return ResourceManager.GetString("PreinstalledPackages_InvalidRegistryValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding {0}.{1} to project....
        /// </summary>
        internal static string PreinstalledPackages_PackageInstallStatus {
            get {
                return ResourceManager.GetString("PreinstalledPackages_PackageInstallStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The preinstalled packages are configured to use a Registry-provided package repository but there was an error accessing Registry key &apos;{0}&apos;..
        /// </summary>
        internal static string PreinstalledPackages_RegistryKeyError {
            get {
                return ResourceManager.GetString("PreinstalledPackages_RegistryKeyError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempting to install version &apos;{1}&apos; of &apos;{0}&apos; but the project already includes a different version. Skipping....
        /// </summary>
        internal static string PreinstalledPackages_VersionConflict {
            get {
                return ResourceManager.GetString("PreinstalledPackages_VersionConflict", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Package Installation Error.
        /// </summary>
        internal static string TemplateWizard_ErrorDialogTitle {
            get {
                return ResourceManager.GetString("TemplateWizard_ErrorDialogTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The project template lists one or more packages with missing, empty, or invalid values for the &quot;id&quot; or &quot;version&quot; attributes. Both attributes are required and must have valid values..
        /// </summary>
        internal static string TemplateWizard_InvalidPackageElementAttributes {
            get {
                return ResourceManager.GetString("TemplateWizard_InvalidPackageElementAttributes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;repository&quot; attribute of the package element has an invalid value: &apos;{0}&apos;. Valid values are: &apos;template&apos; or &apos;extension&apos;..
        /// </summary>
        internal static string TemplateWizard_InvalidRepositoryAttribute {
            get {
                return ResourceManager.GetString("TemplateWizard_InvalidRepositoryAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This template wizard can only be applied to single-project or project-item templates..
        /// </summary>
        internal static string TemplateWizard_InvalidWizardRunKind {
            get {
                return ResourceManager.GetString("TemplateWizard_InvalidWizardRunKind", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The project template is configured to use an Extension-specific package repository but the Extension ID has not been specified. Use the &quot;repositoryId&quot; attribute to specify the Extension ID..
        /// </summary>
        internal static string TemplateWizard_MissingExtensionId {
            get {
                return ResourceManager.GetString("TemplateWizard_MissingExtensionId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The project template is configured to use a Registry-provided package repository but the Registry value name has not been specified. Use the &quot;keyName&quot; attribute to specify the Registry value..
        /// </summary>
        internal static string TemplateWizard_MissingRegistryKeyName {
            get {
                return ResourceManager.GetString("TemplateWizard_MissingRegistryKeyName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find package &apos;{0}&apos;..
        /// </summary>
        internal static string UnknownPackage {
            get {
                return ResourceManager.GetString("UnknownPackage", resourceCulture);
            }
        }
    }
}
