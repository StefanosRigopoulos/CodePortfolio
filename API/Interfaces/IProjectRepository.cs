using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IProjectRepository
    {
        void Update(Project project);
        Task<Project> GetProjectEntityAsync(string username, string projectname);
        Task<IEnumerable<ProjectDTO>> GetProjectsAsync(string username);
        Task<ProjectDTO> GetProjectAsync(string username, string projectname);
        bool DeleteProjectAsync(Project project);
    }
}