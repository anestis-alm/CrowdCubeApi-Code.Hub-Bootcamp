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
    public class UserManagement : IUserManager
    {
        private CrowdDbContext db;

        public UserManagement(CrowdDbContext _db)
        {
            db = _db;
        }

        public Backer CreateBacker(UserOption backOption)
        {
            Backer backer = new Backer
            {
                FirstName = backOption.FirstName,
                LastName = backOption.LastName,
                Email = backOption.Email,
                Username = backOption.Username,
                Password = backOption.Password,
                Dob = backOption.Dob,
                Role = backOption.Role,
                TotalAmount = 0,
                ProjectFunded = 0,
            };


            db.Backers.Add(backer);
            db.SaveChanges();

            return backer;
        }

        public ProjectCreator CreateProjectCreator(UserOption projCreatOption)
        {
            ProjectCreator projectCreator = new ProjectCreator
            {
                FirstName = projCreatOption.FirstName,
                LastName = projCreatOption.LastName,
                Email = projCreatOption.Email,
                Username = projCreatOption.Username,
                Password = projCreatOption.Password,
                Dob = projCreatOption.Dob,
                Role = projCreatOption.Role,
            };


            db.ProjectCreators.Add(projectCreator);
            db.SaveChanges();

            return projectCreator;
        }

        public Backer FindBackerById(int backerId)
        {
            return db.Backers.Find(backerId);
        }

        public string BackerName(int backerId)
        {
            Backer backer = db.Backers.Find(backerId);
            return backer.FirstName;
        }

        public string ProjectCrName(int projCrId)
        {
            ProjectCreator projectCreator = db.ProjectCreators.Find(projCrId);
            return projectCreator.FirstName;
        }

        public List<Backer> FindBackerByName(UserOption backerOption)
        {
            return db.Backers
                .Where(cust => cust.LastName == backerOption.LastName)
                .Where(cust => cust.FirstName == backerOption.FirstName)
                .ToList();
        }

        public ProjectCreator FindProjectCreatorById(int projCrId)
        {
            return db.ProjectCreators.Find(projCrId);
        }

        public List<ProjectCreator> FindProjectCreatorByName(UserOption projCreatOption)
        {
            return db.ProjectCreators
                .Where(cust => cust.LastName == projCreatOption.LastName)
                .Where(cust => cust.FirstName == projCreatOption.FirstName)
                .ToList();
        }

        public ProjectCreator FindProjectCreatorByLoginDetails(UserOption projCreatOption)
        {
            return db.ProjectCreators
                .Where(pc => pc.Username == projCreatOption.Username)
                .Where(pc => pc.Password == projCreatOption.Password)
                .Where(pc => pc.Role == projCreatOption.Role)
                .FirstOrDefault();
        }

        public Backer FindBackerByLoginDetails(UserOption backerOption)
        {
            return db.Backers
                .Where(b => b.Username == backerOption.Username)
                .Where(b => b.Password == backerOption.Password)
                .Where(b => b.Role == backerOption.Role)
                .FirstOrDefault();
        }

        public Backer UpdateBacker(UserOption backerOption, int backerId)
        {

            Backer backer = db.Backers.Find(backerId);

            if (backerOption.FirstName != null)
                backer.FirstName = backerOption.FirstName;
            if (backerOption.LastName != null)
                backer.LastName = backerOption.LastName;
            if (backerOption.Email != null)
                backer.Email = backerOption.Email;
            if (backerOption.Username != null)
                backer.Username = backerOption.Username;
            if (backerOption.Password != null)
                backer.Password = backerOption.Password;
            if (backerOption.Dob != new DateTime())
                backer.Dob = backerOption.Dob;
            db.SaveChanges();
            return backer;
        }

        public ProjectCreator Update(UserOption projCreatOption, int projCrId)
        {

            ProjectCreator projectCreator = db.ProjectCreators.Find(projCrId);

            if (projCreatOption.FirstName != null)
                projectCreator.FirstName = projCreatOption.FirstName;
            if (projCreatOption.LastName != null)
                projectCreator.LastName = projCreatOption.LastName;
            if (projCreatOption.Email != null)
                projectCreator.Email = projCreatOption.Email;
            if (projCreatOption.Username != null)
                projectCreator.Username = projCreatOption.Username;
            if (projCreatOption.Password != null)
                projectCreator.Password = projCreatOption.Password;
            if (projCreatOption.Dob != new DateTime())
                projectCreator.Dob = projCreatOption.Dob;
            db.SaveChanges();
            return projectCreator;
        }

        public bool DeleteBackerById(int backerId)
        {

            Backer backer = db.Backers.Find(backerId);
            if (backer != null)
            {
                db.Backers.Remove(backer);
                db.SaveChanges();
                return true;
            }
            return false;

        }

        public bool DeleteProjectCreatorById(int id)
        {

            ProjectCreator projectCreator = db.ProjectCreators.Find(id);
            if (projectCreator != null)
            {
                db.ProjectCreators.Remove(projectCreator);
                db.SaveChanges();
                return true;
            }
            return false;

        }

        public decimal BackerTotalAmountSpent(int id)
        {
            Backer backer = db.Backers.Find(id);

            List<PackageFund> pfs = db.PackageFunds
              .Include(pf => pf.Package)
              .Where(pf => pf.Backer.Id == id)
              .ToList();

            decimal total_am = 0;
            foreach (PackageFund pf in pfs)
            {
                total_am += pf.Package.Amount;
            }

            return total_am;
        }
    }
}
