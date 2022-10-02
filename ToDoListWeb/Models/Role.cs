using Microsoft.AspNetCore.Identity;

namespace ToDoListWeb.Models
{
    public class Role : IdentityRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }
}
