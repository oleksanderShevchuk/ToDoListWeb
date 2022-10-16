using System.ComponentModel.DataAnnotations;

namespace ToDoListWeb.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
