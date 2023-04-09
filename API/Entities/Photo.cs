namespace API.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string PublicId { get; set; }

        // Relation properties
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}