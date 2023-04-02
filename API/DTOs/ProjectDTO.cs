namespace API.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string MainPhotoURL { get; set; }

        // Information
        public String ProjectTitle { get; set; }
        public String Language { get; set; }
        public String Description { get; set; }
        public List<ProjectPhotoDTO> ProjectPhotos { get; set; } = new ();
    }
}