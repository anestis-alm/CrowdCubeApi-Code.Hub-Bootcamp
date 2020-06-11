using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdCubeMvc.Models
{
    public class ProjectUpdateOption
    {
        public int ProjectCreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Goal { get; set; }
        public string ThemeImage { get; set; }
        public int projId { get; set; }
    }
}
