using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required] public string Username { get; set; }
        [Required] public string Email { get; set; }
        [Required] [StringLength(12, MinimumLength = 6)] public string Password { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public DateOnly? DateOfBirth { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string CodeLanguage { get; set; }
        public string PhotoURL { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string GitHubURL { get; set; }
        public string LinkedInURL { get; set; }
    }
}
