using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;
        public LikeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Like> GetLike(int projectLikedId, int userLikedId)
        {
            return await _context.Likes.FindAsync(projectLikedId, userLikedId);
        }

        public bool RemoveLike(Like like)
        {
            if (_context.Likes.Remove(like) != null) return true;
            return false;
        }

        public async Task<Project> GetProject(int projectLikedId)
        {
            return await _context.Projects.Include(x => x.LikedByUsers).FirstOrDefaultAsync(x => x.Id == projectLikedId);
        }

        public async Task<AppUser> GetUser(int userLikedId)
        {
            return await _context.Users.Include(x => x.LikedProjects).FirstOrDefaultAsync(x => x.Id == userLikedId);
        }

        public async Task<IEnumerable<ProjectDTO>> GetProjectsLiked(int userId)
        {
            var projects = _context.Projects.Include(p => p.ProjectPhotos).OrderBy(p => p.ProjectName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            likes = likes.Where(like => like.UserLikedId == userId);
            projects = likes.Select(like => like.ProjectLiked);

            return await projects.Select(project => new ProjectDTO{
                Id = project.Id,
                ProjectName = project.ProjectName,
                MainPhotoURL = project.MainPhotoURL,
                ProjectTitle = project.ProjectTitle,
                Language = project.Language,
                LikesCount = project.LikesCount
            }).ToListAsync();
        }
    }
}