using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdCubeApp.Options
{
    public class ProjectOption
    {
        public int ProjectCreatorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Goal { get; set; }
        public string ThemeImage { get; set; }

        //Initialized fields by ProjectManagment: Progress, StartDate, State, CurrentAmount
    }
}
