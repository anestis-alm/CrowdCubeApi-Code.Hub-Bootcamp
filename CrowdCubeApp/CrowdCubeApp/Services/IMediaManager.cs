using CrowdCubeApp.Models;
using CrowdCubeApp.Options;
using System.Collections.Generic;

namespace CrowdCubeApp.Services
{
    public interface IMediaManager
    {
        Media CreateMedia(MediaOption mediaOption);
        List<Media> FindMediaByProjectId(int projId);
    }
}