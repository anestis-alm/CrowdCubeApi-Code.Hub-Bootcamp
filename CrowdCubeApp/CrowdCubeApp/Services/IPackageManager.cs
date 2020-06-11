using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using System.Collections.Generic;

namespace CrowdCubeApp.Services
{
    public interface IPackageManager
    {
        Package AddPackage(PackageOption PckOption);
        decimal[] BackerAmountSpent(int backerId);
        List<Package> BackerRewards(int backerId);
        PackageFund CreatePackageFund(PackageFundOption fundOption);
        Package FindPackageById(int pckgId);
        List<Package> FindPackagesByProjectId(int projId);
        List<Project> FindProjectsFundedByBacker(int backerId);
        bool RemoveFundPackage(PackageOption fundPckOpt);
    }
}