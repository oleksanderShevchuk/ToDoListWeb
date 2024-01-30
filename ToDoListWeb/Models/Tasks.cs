using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoListWeb.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a title.")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Please enter a category.")]
        public string CategoryId { get; set; } = string.Empty;
        [ValidateNever]
        public Category Category { get; set; } = null!;
        [Required(ErrorMessage = "Please enter a due date.")]
        public DateTime? DueDate { get; set; } = DateTime.MinValue;
        [Required(ErrorMessage = "Please enter a status.")]
        public string StatusId { get; set; } = string.Empty;
        [ValidateNever]
        public Status Status { get; set; } = null!;
        public bool Overdue => StatusId == "open" && DueDate == DateTime.Today;
        public string? UserId { get; set; }
    }
}
