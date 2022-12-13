using System;
using System.ComponentModel.DataAnnotations;

namespace To_Do.ViewModels
{
    public class AddToDoTaskViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Title must be 1-50 characters long.")]
        public string Title { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Body text must be 1-1000 characters long.")]
        public string Body { get; set; }
        public bool IsImportant { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public AddToDoTaskViewModel(string body)
        {
            Body = body;
        }
        public AddToDoTaskViewModel()
        {

        }
    }
}
