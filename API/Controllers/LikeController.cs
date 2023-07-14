using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikeController : BaseAPIController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ILikeRepository _likeRepository;
        public LikeController(IProjectRepository projectRepository, ILikeRepository likeRepository)
        {
            _projectRepository = projectRepository;
            _likeRepository = likeRepository;
        }

        [HttpGet]       // TODO Does not get the main photo url with the DTO.
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjectLiked() {
            var projects = await _likeRepository.GetProjectsLiked(int.Parse(User.GetUserId()));
            return Ok(projects);
        }

        [HttpPost("{username}/{projectname}")]
        public async Task<ActionResult> AddLike(string username, string projectname) {
            var userLikedId = int.Parse(User.GetUserId());
            var projectLiked = await _projectRepository.GetProjectEntityAsync(username, projectname);
            var userLiked = await _likeRepository.GetUser(userLikedId);

            if (projectLiked == null) return NotFound();

            if (userLiked.Projects.Contains(projectLiked)) return BadRequest("You can not like your own projects!");

            var like = await _likeRepository.GetLike(projectLiked.Id, userLikedId);

            if (like != null) {     // Unlike
                projectLiked.LikedByUsers.Remove(like);
                userLiked.LikedProjects.Remove(like);
                _likeRepository.RemoveLike(like);
                projectLiked.LikesCount--;

                if (await _projectRepository.SaveAllASync()) return Ok();
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

                if (await _projectRepository.SaveAllASync()) return Ok();
                return BadRequest("Failed to like project!");
            }
        }
    }
}