using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;
        public IMapper _mapper { get; }
        public ProjectRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void Update(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
        }

        public async Task<bool> SaveAllASync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync(string username)
        {
            return await _context.Projects.Where(x => x.AppUser.UserName == username).ProjectTo<ProjectDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<ProjectDTO> GetProjectAsync(string username, string projectname)
        {
            return await _context.Projects.Where(x => x.ProjectName == projectname).ProjectTo<ProjectDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }
    }
}