using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CrowdCubeMvc.Models;
using CrowdCubeApp.Repository;
using CrowdCubeApp.Services;

namespace CrowdCubeMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CrowdDbContext _db;
        private IUserManager _userMng;
        private IProjectManager _projMng;
        private IPackageManager _pkMng;
        private IMediaManager _mediaMng;
        private IProjectStatusManager _psMgnr;


        public HomeController(ILogger<HomeController> logger, CrowdDbContext db,
            IUserManager userMng, IProjectManager projMng, IPackageManager pkMng,
            IMediaManager mediaMng, IProjectStatusManager psMgnr)
        {
            _logger = logger;
            _db = db;
            _userMng = userMng;
            _projMng = projMng;
            _pkMng = pkMng;
            _mediaMng = mediaMng;
            _psMgnr = psMgnr;
        }

        public IActionResult Index()
        {
            ProjectCreatorModels projectCreatorModels = new ProjectCreatorModels()
            {
                Projects = _projMng.FindAvailabeProjects(),
                Trends = _projMng.SortProjectsByTrends()
            };

            return View(projectCreatorModels);
        }

        public IActionResult BackerIndex([FromQuery] int? userId)
        {
            int id = userId ?? 0;

            if (_userMng.FindBackerById(id) == null)
            {
                return NotFound();
            }


            ProjectCreatorModels projectCreatorModels = new ProjectCreatorModels()
            {
                Projects = _projMng.FindAvailabeProjects(),
                Backer = _userMng.FindBackerById(id),
                BackerId = userId,
                Trends = _projMng.SortProjectsByTrends(),
                BackerName = _userMng.BackerName(id)
            };

            return View(projectCreatorModels);
        }

        public IActionResult Explore([FromQuery] int? userId, [FromQuery] string? title, [FromQuery] int? pageSize, [FromQuery]int? pageNumber, [FromQuery]string? category)
        {
            int psize = pageSize ?? 6;
            int pnumber = pageNumber ?? 1;

            int id = userId ?? 0;

            if (_userMng.FindBackerById(id) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new ProjectCreatorModels()
            {
                BackerId = userId,
                Projects = _projMng.FindProjectsByTitle(title, psize, pnumber, category),
                //Trends = _projMng.SortProjectsByTrends(),
                maxPage = _projMng.PagesNum(),
                pageNum = pnumber,
                BackerName = _userMng.BackerName(id)
            };

            return View(projectCreatorModels);
        }

        public IActionResult MyFunds([FromQuery] int? userId)
        {
            int id = userId ?? 0;

            if (_userMng.FindBackerById(id) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new ProjectCreatorModels()
            {
                BackerId = userId,
                BackerAmSpent = _pkMng.BackerAmountSpent(id),
                MyFunds = _pkMng.FindProjectsFundedByBacker(userId ?? 1),
                Rewards = _pkMng.BackerRewards(id),
                BackerName = _userMng.BackerName(id)
                //Trends = _projMng.SortProjectsByTrends()
            };
            var rew = _pkMng.BackerRewards(id);
            return View(projectCreatorModels);
        }

        public IActionResult ProjectCreatorIndex([FromQuery] int? userId)
        {
            int id = userId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new ProjectCreatorModels
            {
                ProjectCreatorId = userId,
                ProjectCreatorName = _userMng.ProjectCrName(id),
                Projects = _projMng.FindProjectCrProjects(userId ?? 1)
            };

            foreach (var project in projectCreatorModels.Projects)
            {
                project.Progress = _projMng.TrackProgressByProjectId(project.Id);
            }

            return View(projectCreatorModels);
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Privacy was selected");
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult AddProject([FromQuery]  int? userId)
        {
            int id = userId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                ProjectCreatorId = userId,
                ProjectCreatorName = _userMng.ProjectCrName(id)
            };
            return View(projectCreatorModels);
        }

        public IActionResult AddPackage([FromQuery] int? projectId, [FromQuery] int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                ProjectId = projectId,
                ProjectCreatorId = userId,
                ProjectCreatorName = _userMng.ProjectCrName(id),
            };
            return View(projectCreatorModels);
        }

        public IActionResult ProjectCreatorProjectView([FromQuery] int? projectId, [FromQuery] int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                ProjectCreatorId = userId,
                SingleProject = _projMng.FindProjectById(projectId ?? 1),
                Medias = _mediaMng.FindMediaByProjectId(projectId ?? 1),
                Packages = _pkMng.FindPackagesByProjectId(projectId ?? 1),
                ProjectCreatorName = _userMng.ProjectCrName(id),
                ProjectStatus = _psMgnr.FindStatusesByProjectId(projectId ?? 1)
            };

            projectCreatorModels.Medias.Reverse();
            projectCreatorModels.ProjectStatus.Reverse();

            projectCreatorModels.SingleProject.Progress = _projMng.TrackProgressByProjectId(projectId ?? 1);
            return View(projectCreatorModels);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult AddStatus([FromQuery] int? projectId, int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                ProjectId = projectId,
                ProjectCreatorId = userId,
                ProjectCreatorName = _userMng.ProjectCrName(id)
            };
            return View(projectCreatorModels);
        }

        public IActionResult AddMedia([FromQuery] int? projectId, [FromQuery] int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                ProjectId = projectId,
                ProjectCreatorId = userId,
                ProjectCreatorName = _userMng.ProjectCrName(id)
            };
            return View(projectCreatorModels);
        }

        public IActionResult EditProject([FromQuery] int? projectId, [FromQuery] int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                SingleProject = _projMng.FindProjectById(projectId ?? 1),
                ProjectId = projectId,
                ProjectCreatorId = userId,
                ProjectCreatorName = _userMng.ProjectCrName(id)
            };
            return View(projectCreatorModels);
        }

        public IActionResult ProjectView([FromQuery] int? projectId, [FromQuery] int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindBackerById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                ProjectId = projectId,
                BackerId = userId,
                Project = _projMng.FindProjectById(projectId ?? 1),
                Medias = _mediaMng.FindMediaByProjectId(projectId ?? 1),
                ProjectStatus = _psMgnr.FindStatusesByProjectId(projectId ?? 1),
                BackerName = _userMng.BackerName(id)
            };
            return View(projectCreatorModels);
        }

        public IActionResult CheckOut([FromQuery] int? userId, [FromQuery] int? projectId, [FromQuery] int? packageId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;
            int packId = packageId ?? 0;

            if (_userMng.FindBackerById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                BackerId = userId,
                Package = _pkMng.FindPackageById(packageId ?? 0),
                Project = _projMng.FindProjectById(projectId ?? 1),
                BackerName = _userMng.BackerName(id)
            };
            return View(projectCreatorModels);
        }

        public IActionResult PackageView([FromQuery] int? projectId, [FromQuery] int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindBackerById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new Models.ProjectCreatorModels
            {
                ProjectId = projectId,
                BackerId = userId,
                Packages = _pkMng.FindPackagesByProjectId(projectId ?? 1),
                BackerName = _userMng.BackerName(id)
            };
            return View(projectCreatorModels);
        }

        public IActionResult ProjectBackers([FromQuery] int? projectId, [FromQuery] int? userId)
        {
            int id = userId ?? 0;
            int projId = projectId ?? 0;

            if (_userMng.FindProjectCreatorById(id) == null || _projMng.FindProjectById(projId) == null)
            {
                return NotFound();
            }

            ProjectCreatorModels projectCreatorModels = new ProjectCreatorModels()
            {

                ProjectFunds = _projMng.FindProjectFunds(projectId ?? 1),
                ProjectId = projectId,
                ProjectCreatorId = userId,
                ProjectCreatorName = _userMng.ProjectCrName(id)
                //Trends = _projMng.SortProjectsByTrends()
            };

            return View(projectCreatorModels);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
