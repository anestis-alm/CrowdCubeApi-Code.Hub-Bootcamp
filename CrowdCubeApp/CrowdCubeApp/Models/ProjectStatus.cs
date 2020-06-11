using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdCubeApp.Models
{
    public class ProjectStatus
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public Project Project { get; set; }
    }
}
