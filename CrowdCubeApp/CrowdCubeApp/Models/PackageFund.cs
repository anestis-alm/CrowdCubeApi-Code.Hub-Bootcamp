using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdCubeApp.Models
{
    public class PackageFund
    {
        public int Id { get; set; }
        public DateTime DateFund { set; get; }
        public Backer Backer { get; set; }
        public Package Package { get; set; }
        public Project Project { get; set; }
    }
}
