using Microsoft.AspNetCore.Identity;


namespace Identity.Models
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

        public AppRole()
        {
        }

        private AppRole(string name, string description)
        {
            Name = name;
            Description = string.IsNullOrWhiteSpace(description) ? "no description" : description;
        }

        public static AppRole Create(string name, string description)
        => new(name, description);

        public void ChangeProperties(string name, string description)
        {
            Name = name;
            Description = string.IsNullOrWhiteSpace(description) ? Description : description;
        }

        public void Deactivate() => IsActive = false;

    }
}
