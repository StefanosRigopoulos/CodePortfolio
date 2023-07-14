using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Projects")]
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string MainPhotoURL { get; set; }
        public string PublicId { get; set; }

        // Information
        public String ProjectTitle { get; set; }
        public String Language { get; set; }
        public String Description { get; set; }
        public List<ProjectPhoto> ProjectPhotos { get; set; } = new ();
        public List<Like> LikedByUsers { get; set; } = new ();
        public int LikesCount { get; set; }

        // Relation properties
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

    }
}