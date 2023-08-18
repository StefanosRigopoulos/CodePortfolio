using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikeController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public LikeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjectLiked() {
            var projects = await _uow.LikeRepository.GetProjectsLiked(int.Parse(User.GetUserId()));
            return Ok(projects);
        }

        [HttpPost("{username}/{projectname}")]
        public async Task<ActionResult> AddLike(string username, string projectname) {
            var userLikedId = int.Parse(User.GetUserId());
            var projectLiked = await _uow.ProjectRepository.GetProjectEntityAsync(username, projectname);
            var userLiked = await _uow.LikeRepository.GetUser(userLikedId);

            if (projectLiked == null) return NotFound();

            if (userLiked.Projects.Contains(projectLiked)) return BadRequest("You can not like your own projects!");

            var like = await _uow.LikeRepository.GetLike(projectLiked.Id, userLikedId);

            if (like != null) {     // Unlike
                projectLiked.LikedByUsers.Remove(like);
                userLiked.LikedProjects.Remove(like);
                _uow.LikeRepository.RemoveLike(like);
                projectLiked.LikesCount--;

                if (await _uow.Complete()) return Ok();
                return BadRequest("Failed to unlike project!");
            }
            else {                  // Like
                like = new Like {
                    UserLikedId = userLikedId,
                    ProjectLikedId = projectLiked.Id
                };
                projectLiked.LikedByUsers.Add(like);
                userLiked.LikedProjects.Add(like);
                projectLiked.LikesCount++;

                if (await _uow.Complete()) return Ok();
                return BadRequest("Failed to like project!");
            }
        }
    }
}