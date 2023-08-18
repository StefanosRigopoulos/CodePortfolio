using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ProjectsController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;

        public ProjectsController(IUnitOfWork uow, IMapper mapper, IPhotoService photoService)
        {
            _uow = uow;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects(string username)
        {
            var projects = await _uow.ProjectRepository.GetProjectsAsync(username);
            return Ok(projects);
        }

        [HttpGet("{username}/{projectname}")]
        public async Task<ActionResult<ProjectDTO>> GetProject(string username, string projectname)
        {
            return await _uow.ProjectRepository.GetProjectAsync(username, projectname);
        }

        [HttpPut("{username}/{projectname}/edit")]
        public async Task<ActionResult> UpdateProject(string username, string projectname, ProjectUpdateDTO projectUpdateDTO)
        {
            // Getting the project.
            var project = await _uow.ProjectRepository.GetProjectEntityAsync(username, projectname);
            if (project == null) return NotFound();

            // Mapping the received DTO to our project entity.
            _mapper.Map(projectUpdateDTO, project);

            // Saving changes.
            if (await _uow.Complete()) return NoContent();
            return BadRequest("Failed to update project!");
        }

        [HttpPost("{username}/{projectname}/add-project-photo/")]
        public async Task<ActionResult<ProjectPhotoDTO>> AddProjectPhoto(string username, string projectname, IFormFile file)
        {
            // Getting the project.
            var project = await _uow.ProjectRepository.GetProjectEntityAsync(username, projectname);
            if (project == null) return NotFound();

            // Upload new profile photo.
            var result = await _photoService.AddProjectPhotoAsync(username, projectname, file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            // Creating a new Photo object.
            var photo = new ProjectPhoto
            {
                URL = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            // Checking if this is the first photo of the project and if it is we make it main.
            if (project.ProjectPhotos.Count == 0) photo.IsMain = true;

            // Adding the photo entity to our table of project photos.
            project.ProjectPhotos.Add(photo);

            // Saving changes.
            if (await _uow.Complete())
            {
                return _mapper.Map<ProjectPhotoDTO>(photo);
            }
            return BadRequest("Problem adding photo!");
        }

        [HttpDelete("{username}/{projectname}/delete")]
        public async Task<ActionResult> DeleteProject(string username, string projectname)
        {
            // Getting the project.
            var project = await _uow.ProjectRepository.GetProjectEntityAsync(username, projectname);
            if (project == null) return NotFound();

            if (!_uow.ProjectRepository.DeleteProjectAsync(project)) return BadRequest("Project can not be deleted from the repository!");
            await _photoService.DeleteProjectFolder(username, projectname);

            // Saving changes.
            if (await _uow.Complete()) return Ok();
            return BadRequest("Problem deleting project!");
        }

        [HttpPut("{username}/{projectname}/set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainProjectPhoto(string username, string projectname, int photoId)
        {
            // Getting the project.
            var project = await _uow.ProjectRepository.GetProjectEntityAsync(username, projectname);
            if (project == null) return NotFound();

            // Getting the photo with the certain ID.
            var photo = project.ProjectPhotos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();

            // Check if photo is already main.
            if (photo.IsMain) return BadRequest("This is already your main photo!");

            // Configuring the photo to be the main.
            var currentMain = project.ProjectPhotos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            // Pushing changes to the database.
            if (await _uow.Complete()) return NoContent();
            return BadRequest("Problem setting the main photo!");
        }

        [HttpDelete("{username}/{projectname}/delete-photo/{photoId}")]
        public async Task<ActionResult> DeleteProjectPhoto(string username, string projectname, int photoId)
        {
            // Getting the project.
            var project = await _uow.ProjectRepository.GetProjectEntityAsync(username, projectname);
            if (project == null) return NotFound();

            // Getting the photo with the certain ID.
            var photo = project.ProjectPhotos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();

            // Checking if this photo is the main.
            if (photo.IsMain) return BadRequest("You cannot delete your main photo!");

            // Checking if its a photo not uploaded to the cloud.
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            // Removing the photo.
            project.ProjectPhotos.Remove(photo);

            // Saving changes.
            if (await _uow.Complete()) return Ok();
            return BadRequest("Problem deleting photo!");
        }
    }
}