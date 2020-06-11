using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using CrowdCubeApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CrowdCubeMvc.Controllers
{
    public class ProjectController : Controller
    {
        private IUserManager userMngr;
        private IProjectManager projectMngr;
        private IPackageManager packageMngr;
        private IMediaManager mediaMngr;
        private IProjectStatusManager psMgnr;

        public ProjectController(IUserManager _userMngr, IProjectManager _projectMngr
            , IPackageManager _packageMngr, IMediaManager _mediaMngr, IProjectStatusManager _psMgnr)
        {
            userMngr = _userMngr;
            projectMngr = _projectMngr;
            packageMngr = _packageMngr;
            mediaMngr = _mediaMngr;
            psMgnr = _psMgnr;
        }

        [HttpPost]
        public int[] AddMedia([FromBody] MediaOption mediaOption)
        {
            Media media = mediaMngr.CreateMedia(mediaOption);
            int userId = projectMngr.FindProjectCreatorByProjectId(media.Project.Id);
            int[] redirect = { media.Project.Id, userId };
            return redirect;
        }

        [HttpPost]
        public int[] AddStatus([FromBody] ProjectStatusOption psOption)
        {
            ProjectStatus ps = psMgnr.CreateProjectStatus(psOption);
            int userId = projectMngr.FindProjectCreatorByProjectId(ps.Project.Id);
            int[] redirect = { ps.Project.Id, userId };
            return redirect;
        }
    }
}