using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IProjectRepository
    {
        void Update(Project project);
        Task<bool> SaveAllASync();
        Task<IEnumerable<ProjectDTO>> GetProjectsAsync(string username);
        Task<ProjectDTO> GetProjectAsync(string username, string projectname);
    }
}