using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using CrowdCubeApp.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrowdCubeApp.Services
{
    public class PackageManagement : IPackageManager
    {
        private CrowdDbContext db;

        public PackageManagement(CrowdDbContext _db)
        {
            db = _db;
        }



        public PackageFund CreatePackageFund(PackageFundOption fundOption)
        {
            PackageFund fund = new PackageFund
            {
                Backer = db.Backers.Find(fundOption.BackerId),
                Package = db.Packages.Find(fundOption.PackageId),
                Project = db.Projects.Find(fundOption.ProjectId),
                DateFund = DateTime.Now
            };

            Project project = db.Projects.Find(fundOption.ProjectId);
            project.CurrentAmount += fund.Package.Amount;
            project.NumberOfBackers += 1;
            if (project.CurrentAmount >= project.Goal)
            {
                project.State = false;
            }

            ProjectManagement prjMng = new ProjectManagement(db);
            project.Progress = prjMng.TrackProgressByProjectId(fundOption.ProjectId);

            db.Projects.Update(project);
            db.PackageFunds.Add(fund);
            db.SaveChanges();

            PackageManagement pckMng = new PackageManagement(db);
            UserManagement userMng = new UserManagement(db);
            Backer backer = db.Backers.Find(fundOption.BackerId);
            backer.ProjectFunded = pckMng.FindProjectsFundedByBacker(fundOption.BackerId).Count;


            backer.TotalAmount = userMng.BackerTotalAmountSpent(fundOption.BackerId);

            db.Backers.Update(backer);
            db.SaveChanges();


            return fund;
        }


        public Package AddPackage(PackageOption PckOption)
        {

            Package package = new Package
            {
                Amount = PckOption.Amount,
                Reward = PckOption.Reward,
                Description = PckOption.Description,
                Project = db.Projects.Find(PckOption.ProjectId)
            };

            db.Packages.Add(package);
            db.SaveChanges();
            return package;
        }

        public List<Package> FindPackagesByProjectId(int projId)
        {
            return db.Packages
                .Where(pk => pk.Project.Id == projId).ToList();
        }

        public Package FindPackageById(int pckgId)
        {
            return db.Packages
                .Where(pk => pk.Id == pckgId).SingleOrDefault();
        }

        public decimal[] BackerAmountSpent(int backerId)
        {
            List<Project> pjs = FindProjectsFundedByBacker(backerId);
            List<PackageFund> pfs = db.PackageFunds
                .Include(pf => pf.Package)
                .Include(pf => pf.Project)
                .Where(pf => pf.Backer.Id == backerId)
                .ToList();

            int count = 0;
            decimal[] amount = new decimal[pjs.Count];
            foreach (Project pj in pjs)
            {
                foreach (PackageFund pf in pfs)
                {
                    if (pj.Id == pf.Project.Id)
                    {
                        amount[count] += pf.Package.Amount;
                    }

                }
                count++;
            }


            return amount;
        }

        public List<Package> BackerRewards(int backerId)
        {

            List<Project> pjs = FindProjectsFundedByBacker(backerId);

            List<PackageFund> pfs = db.PackageFunds
                 .Include(pf => pf.Package)
                 .Include(pf => pf.Project)
                 .Where(pf => pf.Backer.Id == backerId)
                 .ToList();

            List<Package> listPack = new List<Package>();

            foreach (Project pj in pjs)
            {
                foreach (PackageFund pf in pfs)
                {
                    if (pj.Id == pf.Project.Id)
                    {
                        listPack.Add(pf.Package);
                    }

                }

            }
            return listPack;
        }


        public List<Project> FindProjectsFundedByBacker(int backerId)
        {

            List<PackageFund> pfs = db.PackageFunds
                .Include(pf => pf.Project)
                .Where(pf => pf.Backer.Id == backerId)
                .ToList();

            List<Project> proj = new List<Project>();
            List<Project> projects = new List<Project>();

            foreach (PackageFund pf in pfs)
            {
                proj.Add(pf.Project);
            }

            foreach (Project pr in proj)
            {
                if (projects.Contains(pr) == false)
                {
                    projects.Add(pr);
                }
            }

            return projects;
        }


        public bool RemoveFundPackage(PackageOption fundPckOpt)
        {
            PackageFund fundPckg = db.PackageFunds.Find(fundPckOpt);

            if (fundPckg != null)
            {
                db.PackageFunds.Remove(fundPckg);
                db.SaveChanges();
                return true;
            }

            return false;
        }

    }
}
