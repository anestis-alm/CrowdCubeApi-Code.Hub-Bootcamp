using CrowdCubeApp.Repository;
using CrowdCubeApp.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace CrowdCubeApp
{
    class Program
    {
        static void Main()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CrowdDbContext>();
            optionsBuilder.UseSqlServer(CrowdDbContext.ConnectionString);
            using CrowdDbContext db = new CrowdDbContext(optionsBuilder.Options);

            IPackageManager projectMngr = new PackageManagement(db);
            var list = projectMngr.BackerRewards(3);

            IProjectManager project = new ProjectManagement(db);
            int c = project.PagesNum();

        }
    }
}
