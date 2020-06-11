using CrowdCubeApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdCubeMvc.Models
{
    public class ProjectCreatorModels
    {
        public int? ProjectCreatorId { get; set; }
        public string ProjectCreatorName { get; set; }
        public string BackerName { get; set; }
        public Project Project { get; set; }
        public Package Package { get; set; }
        public DateTime Now { get; }
        public int pageNum { get; set; }
        public int? ProjectId { get; set; }
        public int? BackerId { get; set; }
        public Backer Backer { get; set; }
        public Project SingleProject { get; set; }
        public decimal[] BackerAmSpent { get; set; }
        public List<Project> Projects { get; set; }
        public List<Package> Packages { get; set; }
        public List<Media> Medias { get; set; }
        public List<Project> Trends { get; set; }
        public List<Project> MyFunds { get; set; }
        public List<ProjectStatus> ProjectStatus { get; set; }
        public List<Package> Rewards { get; set; }
        public List<PackageFund> ProjectFunds { get; set; }
        public IFormFile ThemeImage { set; get; }
        public Categories Categories { get; set; }

        public int maxPage { get; set; }
    }
}
