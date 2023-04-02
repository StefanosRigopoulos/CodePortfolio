using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("ProjectPhotos")]
    public class ProjectPhoto
    {   
        public int Id { get; set; }
        public string URL { get; set; }
        public string PublicId { get; set; }

        // Relation properties
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}