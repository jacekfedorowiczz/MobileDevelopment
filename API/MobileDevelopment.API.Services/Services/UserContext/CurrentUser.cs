namespace MobileDevelopment.API.Services.Services.UserContext
{
    public class CurrentUser(string id, string email, IEnumerable<string> roles)
    {
        public string Id { get; set; } = id;
        public string Email { get; set; } = email;
        public IEnumerable<string> Roles { get; set; } = roles;

        public bool IsInRole(string role) => Roles.Contains(role);
    }
}
