using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using AMF.Tools.Properties;
using NuGet.VisualStudio;

namespace MuleSoft.RAML.Tools
{
    public static class NugetInstallerHelper
    {
        public static void InstallPackageIfNeeded(Project proj, IEnumerable<IVsPackageMetadata> packs, IVsPackageInstaller installer, 
            string packageId, string packageVersion, string nugetPackagesSource = null)
        {
            var packageMetadata = packs.FirstOrDefault(p => p.Id == packageId);
            if (packageMetadata == null ||
                !IsSameOrNewerVersion(packageMetadata.VersionString, packageVersion))
            {
                if (nugetPackagesSource == null)
                    nugetPackagesSource = Settings.Default.NugetPackagesSource;

                installer.InstallPackage(nugetPackagesSource, proj, packageId, packageVersion, false);
            }
        }

        private static bool IsSameOrNewerVersion(string installedVersion, string minimumVersion)
        {
            var installedVersionArr = installedVersion.Split('.');
            var minimumVersionArr = minimumVersion.Split('.');

            for (var i = 0; i < installedVersionArr.Length; i++)
            {
                if (Convert.ToInt16(installedVersionArr[i]) > Convert.ToInt16(minimumVersionArr[i]))
                    return true;

                if (Convert.ToInt16(installedVersionArr[i]) < Convert.ToInt16(minimumVersionArr[i]))
                    return false;
            }
            return true;
        }
    }
}