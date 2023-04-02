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
        private readonly IProjectRepository _projectRepository;
        public IMapper _mapper { get; }
        public ProjectsController(IProjectRepository projectRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects(string username)
        {
            var projects = await _projectRepository.GetProjectsAsync(username);
            return Ok(projects);
        }

        [HttpGet("{username}/{projectname}")]
        public async Task<ActionResult<ProjectDTO>> GetProject(string username, string projectname)
        {
            return await _projectRepository.GetProjectAsync(username, projectname);
        }
    }
}