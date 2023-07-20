namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string CodeLanguage { get; set; }
        public string OrderBy { get; set; } = "lastActive";
    }
}