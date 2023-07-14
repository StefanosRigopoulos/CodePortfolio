namespace API.Entities
{
    public class Like
    {
        public AppUser UserLiked { get; set; }
        public int UserLikedId { get; set; }
        public Project ProjectLiked { get; set; }
        public int ProjectLikedId { get; set; }
    }
}