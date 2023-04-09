using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(string username, IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        Task<ImageUploadResult> AddProjectPhotoAsync(string username, string projectname, IFormFile file);
    }
}