using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdCubeApp.Models
{
    public class Backer : User
    {
        public int ProjectFunded { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PackageFund> Funds { get; set; }
    }
}
