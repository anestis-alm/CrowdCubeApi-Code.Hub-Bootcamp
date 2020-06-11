using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using System.Collections.Generic;

namespace CrowdCubeApp.Services
{
    public interface IUserManager
    {
        string BackerName(int backerId);
        decimal BackerTotalAmountSpent(int id);
        Backer CreateBacker(UserOption backOption);
        ProjectCreator CreateProjectCreator(UserOption projCreatOption);
        bool DeleteBackerById(int backerId);
        bool DeleteProjectCreatorById(int id);
        Backer FindBackerById(int backerId);
        Backer FindBackerByLoginDetails(UserOption backerOption);
        List<Backer> FindBackerByName(UserOption backerOption);
        ProjectCreator FindProjectCreatorById(int projCrId);
        ProjectCreator FindProjectCreatorByLoginDetails(UserOption projCreatOption);
        List<ProjectCreator> FindProjectCreatorByName(UserOption projCreatOption);
        string ProjectCrName(int projCrId);
        ProjectCreator Update(UserOption projCreatOption, int projCrId);
        Backer UpdateBacker(UserOption backerOption, int backerId);
    }
}