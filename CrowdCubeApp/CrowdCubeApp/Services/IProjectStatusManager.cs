using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using System.Collections.Generic;

namespace CrowdCubeApp.Services
{
    public interface IProjectStatusManager
    {
        ProjectStatus CreateProjectStatus(ProjectStatusOption psOption);
        List<ProjectStatus> FindStatusesByProjectId(int projId);
    }
}