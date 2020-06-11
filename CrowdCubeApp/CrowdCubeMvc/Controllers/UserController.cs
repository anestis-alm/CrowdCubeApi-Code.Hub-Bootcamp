using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CFMvcProj.Models;
using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using CrowdCubeApp.Services;
using CrowdCubeMvc.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CrowdCubeMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private IUserManager userMngr;
        private IProjectManager projectMngr;
        private IPackageManager packageMngr;
        //damcomment
        public UserController(IUserManager _userMngr, IProjectManager _projectMngr, IPackageManager _packageMngr, IWebHostEnvironment environment)
        {
            hostingEnvironment = environment;
            userMngr = _userMngr;
            projectMngr = _projectMngr;
            packageMngr = _packageMngr;
        }

        [HttpPost]
        public Backer AddBacker([FromBody] UserOption backOpt)
        {
            return userMngr.CreateBacker(backOpt);
        }

        [HttpPost]
        public ProjectCreator AddProjectCreator([FromBody] UserOption ProjCrOpt)
        {
            return userMngr.CreateProjectCreator(ProjCrOpt);
        }

        [HttpPost]
        public int[] AddProject([FromBody] ProjectOption projOption)
        {
            Project project = projectMngr.CreateProject(projOption);
            int[] redirect = { project.Id, projOption.ProjectCreatorId };
            return redirect;
        }

        [HttpPost]
        public int[] EditProject([FromBody] ProjectUpdateOption projOption)
        {
            ProjectOption projectOption = new ProjectOption
            {
                Title = projOption.Title,
                Description = projOption.Description,
                Category = projOption.Category,
                EndDate = projOption.EndDate,
                Goal = projOption.Goal,
                ProjectCreatorId = projOption.ProjectCreatorId,
                ThemeImage = projOption.ThemeImage
            };
            Project project = projectMngr.Update(projectOption, projOption.projId);
            int[] redirect = { project.Id, projOption.ProjectCreatorId };
            return redirect;
        }


        [HttpPost]
        public int AddPackage([FromBody] PackageOption pckgOption)
        {
            Package package = packageMngr.AddPackage(pckgOption);
            return package.Id;
        }

        [HttpPost]
        public ProjectCreator LoginProjectCreator([FromBody] UserOption ProjCrOpt)
        {
            var pc = userMngr.FindProjectCreatorByLoginDetails(ProjCrOpt);
            return pc;
        }

        [HttpPost]
        public Backer LoginBacker([FromBody] UserOption backOpt)
        {
            var backer = userMngr.FindBackerByLoginDetails(backOpt);
            return backer;
        }

        public int AddFund([FromBody] PackageFundOption fundOption)
        {
            PackageFund packageFund = packageMngr.CreatePackageFund(fundOption);
            return packageFund.Id;
        }


        [HttpPost]
        public ReturnModel CreateUsingAjaX([FromForm]   ProjectCreatorModels model)
        {

            // do other validations on your model as needed
            if (model.ThemeImage != null)
            {
                //var uniqueFileName = GetUniqueFileName(model.ThemeImage.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "assets/img");
                var filePath = Path.Combine(uploads, model.ThemeImage.FileName);
                model.ThemeImage.CopyTo(new FileStream(filePath, FileMode.Create));

                //to do : Save uniqueFileName  to your db table   


                return new ReturnModel { ReturnValue = 1 };
            }

            else return new ReturnModel { ReturnValue = -1 };
        }

        //private string GetUniqueFileName(string fileName)
        //{
        //    fileName = Path.GetFileName(fileName);
        //    return Path.GetFileNameWithoutExtension(fileName)
        //              + "_"
        //              + Guid.NewGuid().ToString().Substring(0, 4)
        //              + Path.GetExtension(fileName);
        //}


    }
}