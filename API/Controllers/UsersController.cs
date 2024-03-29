using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;
        public UsersController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _photoService = photoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MemberDTO>>> GetUsers([FromQuery]UserParams userParams)
        {
            userParams.CurrentUsername = User.GetUsername();
            if (string.IsNullOrEmpty(userParams.CodeLanguage)) { }
            var users = await _uow.UserRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }

        [HttpGet("{username}/")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return await _uow.UserRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            // Getting the user from our repository.
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            // Mapping the received DTO to our user entity.
            _mapper.Map(memberUpdateDTO, user);

            // Saving changes.
            if (await _uow.Complete()) return NoContent();
            return BadRequest("Failed to update user!");
        }

        [HttpPost("add-profile-photo")]
        public async Task<ActionResult<PhotoDTO>> AddProfilePhoto(IFormFile file)
        {
            // Getting the user from our repository.
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            // Delete previous profile photo from our database.
            if (user.Photo != null) {
                if (user.Photo.PublicId != null) {
                    var delete = await _photoService.DeletePhotoAsync(user.Photo.PublicId);
                    if (delete.Error != null) return BadRequest(delete.Error.Message);
                }
            }

            // Upload new profile photo.
            var result = await _photoService.AddPhotoAsync(User.GetUsername(), file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            // Creating a new Photo object.
            var photo = new Photo
            {
                URL = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            // Because we have only one Photo object
            user.Photo = photo;

            // Saving changes.
            if (await _uow.Complete())
            {
                return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, _mapper.Map<PhotoDTO>(photo));
            }
            return BadRequest("Problem adding photo!");
        }

        [HttpPost("create-project")]
        public async Task<ActionResult<ProjectDTO>> CreateProject(ProjectDTO projectDTO)
        {
            // Getting the user from our repository.
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            // Creating a new project entity.
            var project = new Project
            {
                ProjectName = projectDTO.ProjectName,
                ProjectTitle = projectDTO.ProjectTitle,
                Language = projectDTO.Language,
                Description = projectDTO.Description,
                ProjectPhotos = new List<ProjectPhoto>()
            };

            // Adding the photo entity to our table of project photos.
            user.Projects.Add(project);

            // Saving changes.
            if (await _uow.Complete()) return NoContent();
            return BadRequest("Failed to create a new project!");
        }
    }
}