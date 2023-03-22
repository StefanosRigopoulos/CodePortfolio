using API.Entities;

namespace API.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string MainPhotoURL { get; set; }
        public string Email { get; set; }
        public int age { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public List<PhotoDTO> Photos { get; set; } = new ();

        // More Info
        public string Bio { get; set; }
        public string SocialURL_1 { get; set; }
        public string SocialURL_2 { get; set; }
        public string SocialURL_3 { get; set; }
        public string SocialURL_4 { get; set; }
    }
}