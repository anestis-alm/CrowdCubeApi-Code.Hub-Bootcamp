using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using System.Collections.Generic;

namespace CrowdCubeApp.Services
{
    public interface IProjectManager
    {
        Project CreateProject(ProjectOption projOption);
        List<Project> FindAvailabeProjects();
        Project FindProjectById(int projId);
        int FindProjectCreatorByProjectId(int projId);
        List<Project> FindProjectCrProjects(int projCrId);
        List<PackageFund> FindProjectFunds(int projId);
        List<Project> FindProjects();
        List<Project> FindProjectsByTitle(string title, int pageSize, int pageNumber, string category);
        bool HardDeleteProjectById(int id);
        int PagesNum();
        bool SoftDeleteProjectById(int id);
        List<Project> SortProjectsByTrends();
        decimal TrackProgressByProjectId(int projId);
        Project Update(ProjectOption projOption, int projId);
    }
}