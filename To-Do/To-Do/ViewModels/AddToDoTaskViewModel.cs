using System.ComponentModel.DataAnnotations;

namespace To_Do.ViewModels
{
    public class AddToDoTaskViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Title must be 1-50 characters long.")]
        public string Title { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Body text must be 1-1000 characters long.")]
        public string Body { get; set; }
    }
}
