using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdCubeApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Progress { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool State { get; set; }
        public decimal Goal { get; set; }
        public decimal CurrentAmount { get; set; }
        public ProjectCreator ProjectCreator { get; set; }
        public List<Media> Medias { get; set; }
        public List<ProjectStatus> ProjectStatuses { get; set; }
        public List<Package> Packages { get; set; }
        public int NumberOfBackers { get; set; }
        public string ThemeImage { get; set; }

    }
}
