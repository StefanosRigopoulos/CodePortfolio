using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like> GetLike(int projectLikedId, int userLikedId);
        bool RemoveLike(Like like);
        Task<Project> GetProject(int projectLikedId);
        Task<AppUser> GetUser(int userLikedId);
        Task<IEnumerable<ProjectDTO>> GetProjectsLiked(int userId);
    }
}