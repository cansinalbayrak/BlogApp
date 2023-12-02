namespace WebApp.Models
{
    public class UserRolesAddViewModel
    {
        public string Id { get; set; }
        public List<AssignRoleViewModel> Roles { get; set; } = new();
    }
}
