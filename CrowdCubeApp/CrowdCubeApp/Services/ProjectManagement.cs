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
    public class ProjectManagement : IProjectManager
    {
        private CrowdDbContext db;

        public ProjectManagement(CrowdDbContext _db)
        {
            db = _db;
        }

        //CRUD
        // create read update delete

        public Project CreateProject(ProjectOption projOption)
        {
            UserManagement pcMng = new UserManagement(db);
            Project project = new Project
            {
                ProjectCreator = pcMng.FindProjectCreatorById(projOption.ProjectCreatorId),
                Title = projOption.Title,
                Description = projOption.Description,
                Category = projOption.Category,
                EndDate = projOption.EndDate,
                Goal = projOption.Goal,
                ThemeImage = projOption.ThemeImage,
                Progress = 0,
                StartDate = DateTime.Now,
                State = true,
                CurrentAmount = 0,
                NumberOfBackers = 0
            };

            db.Projects.Add(project);
            db.SaveChanges();

            return project;
        }

        public Project FindProjectById(int projId)
        {
            return db.Projects.Find(projId);
        }

        public int FindProjectCreatorByProjectId(int projId)
        {
            var project = db.Projects.Include(p => p.ProjectCreator).Where(p => p.Id == projId).FirstOrDefault();
            return project.ProjectCreator.Id;
        }

        public List<Project> FindProjectCrProjects(int projCrId)
        {
            return db.Projects
                .Where(project => project.ProjectCreator.Id == projCrId)
                .ToList();
        }

        public List<Project> FindProjects()
        {
            return db.Projects.ToList();
        }

        public List<Project> FindAvailabeProjects()
        {
            return db.Projects.
                Where(p => p.State == true)
                .ToList();
        }

        //public List<Project> FindProjectsByCategory(string category)
        //{
        //    return db.Projects.
        //        Where(p => p.Category == category)
        //        .ToList();
        //}

        public List<Project> FindProjectsByTitle(string title, int pageSize, int pageNumber, string category)
        {
            if (title == null && category == null)
            {
                return FindAvailabeProjects().Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            else if (title == null && category != null)
            {
                return FindAvailabeProjects().
                Where(p => p.Category == category)
                .ToList();
            }
            else
            {
                return FindAvailabeProjects()
                    .Where(p => p.Title.ToLower().Contains(title.ToLower()))
                    .Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
        }

        public int PagesNum()
        {
            List<Project> projects = FindAvailabeProjects();
            var projNum = projects.Count();
            int pageCount = (projNum + 6 - 1) / 6;
            return pageCount;
        }

        public List<Project> SortProjectsByTrends()
        {
            List<Project> projectsNotSorted = FindAvailabeProjects();
            foreach (Project p in projectsNotSorted)
            {
                p.NumberOfBackers = db.PackageFunds
                    .Where(pf => pf.Project.Id == p.Id)
                    .ToList().Count();
            }

            return projectsNotSorted.OrderByDescending(p => p.NumberOfBackers).ToList();
        }

        public decimal TrackProgressByProjectId(int projId)
        {
            Project project = FindProjectById(projId);
            return (Math.Round((project.CurrentAmount / project.Goal) * 100m, 2));
        }

        public Project Update(ProjectOption projOption, int projId)
        {
            Project project = db.Projects.Find(projId);

            if (projOption.Title != null)
                project.Title = projOption.Title;
            if (projOption.Description != null)
                project.Description = projOption.Description;
            if (projOption.Category != null)
                project.Category = projOption.Category;
            if (projOption.EndDate != new DateTime())
                project.EndDate = projOption.EndDate;
            if (projOption.Goal > 0)
            {
                project.Goal = projOption.Goal;

                if (project.CurrentAmount < project.Goal)
                {
                    project.State = true;
                }
                else
                {
                    project.State = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(projOption.ThemeImage))
                project.ThemeImage = projOption.ThemeImage;


            db.SaveChanges();

            project.Progress = TrackProgressByProjectId(projId);
            db.Projects.Update(project);
            db.SaveChanges();

            return project;
        }

        public List<PackageFund> FindProjectFunds(int projId)
        {
            return db.PackageFunds
                .Include(pf => pf.Backer)
                .Include(pf => pf.Package)
                .Where(pf => pf.Project.Id == projId)
                .ToList();
        }


        //Delete Project: Hard
        public bool HardDeleteProjectById(int id)
        {
            Project project = db.Projects.Find(id);
            if (project != null)
            {
                db.Projects.Remove(project);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        //Delete Project: Soft
        public bool SoftDeleteProjectById(int id)
        {
            Project project = db.Projects.Find(id);
            if (project != null)
            {
                project.State = false;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
