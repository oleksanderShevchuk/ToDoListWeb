using Microsoft.AspNetCore.Identity;

namespace ToDoListWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
