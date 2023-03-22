using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        //Register Form
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; } = new ();

        // More Info
        public string Bio { get; set; }
        public string SocialURL_1 { get; set; }
        public string SocialURL_2 { get; set; }
        public string SocialURL_3 { get; set; }
        public string SocialURL_4 { get; set; }
    }
}