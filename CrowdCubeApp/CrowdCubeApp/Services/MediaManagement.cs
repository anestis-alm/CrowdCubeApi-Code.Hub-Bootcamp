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
    public class MediaManagement : IMediaManager
    {
        private CrowdDbContext db;

        public MediaManagement(CrowdDbContext _db)
        {
            db = _db;
        }

        public Media CreateMedia(MediaOption mediaOption)
        {
            Project project = db.Projects.Find(mediaOption.ProjectId);

            Media media = new Media
            {
                Type = mediaOption.Type,
                URL = mediaOption.URL,
                Project = project
            };

            db.Medias.Add(media);
            db.SaveChanges();

            return media;
        }

        public List<Media> FindMediaByProjectId(int projId)
        {
            return db.Medias
                .Include(media => media.Project)
                .Where(media => media.Project.Id == projId)
                .ToList();
        }
    }
}
