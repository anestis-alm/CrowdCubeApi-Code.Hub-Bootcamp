using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdCubeApp.Models
{
    public class ProjectCreator : User
    {
        public List<Project> Projects { get; set; }
    }
}
