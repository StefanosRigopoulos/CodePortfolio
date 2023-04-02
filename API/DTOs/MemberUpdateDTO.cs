namespace API.DTOs
{
    public class MemberUpdateDTO
    {
        public string NickName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string CodeLanguage { get; set; }
        public string Bio { get; set; }
        public string FacebookURL { get; set; }
        public string TwitterURL { get; set; }
        public string GitHubURL { get; set; }
        public string LinkedInURL { get; set; }
        public string ProfilePhotoURL { get; set; }
    }
}