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
    public class ProjectStatusManagement : IProjectStatusManager
    {
        private CrowdDbContext db;
        public ProjectStatusManagement(CrowdDbContext _db)
        {
            db = _db;
        }
        public ProjectStatus CreateProjectStatus(ProjectStatusOption psOption)
        {
            Project project = db.Projects.Find(psOption.ProjectId);
            ProjectStatus ps = new ProjectStatus
            {
                Title = psOption.Title,
                Project = project,
                Date = DateTime.Now
            };
            db.ProjectStatuses.Add(ps);
            db.SaveChanges();

            return ps;
        }

        public List<ProjectStatus> FindStatusesByProjectId(int projId)
        {
            return db.ProjectStatuses
                .Include(ps => ps.Project)
                .Where(ps => ps.Project.Id == projId)
                .ToList();
        }
    }
}
