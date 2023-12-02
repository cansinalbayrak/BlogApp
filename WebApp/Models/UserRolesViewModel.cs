namespace WebApp.Models
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }
        public UserViewModel User { get; set; } = new UserViewModel();
        public List<AssignRoleViewModel> Roles { get; set; } = new();
    }
}
