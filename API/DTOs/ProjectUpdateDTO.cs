namespace API.DTOs
{
    public class ProjectUpdateDTO
    {
        public string MainPhotoURL { get; set; }

        // Information
        public String ProjectTitle { get; set; }
        public String Language { get; set; }
        public String Description { get; set; }
    }
}