using System;
using System.ComponentModel.DataAnnotations;

namespace To_Do.ViewModels
{
    public class AddFolderViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Folder name must be between 1-50 characters long.")]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
