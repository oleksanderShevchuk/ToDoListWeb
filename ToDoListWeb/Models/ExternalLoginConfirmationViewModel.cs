﻿using System.ComponentModel.DataAnnotations;

namespace ToDoListWeb.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
