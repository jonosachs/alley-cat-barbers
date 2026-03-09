using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AlleyCatBarbers.ViewModels
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "The To field is required.")]
        public string To { get; set; }

        [Required(ErrorMessage = "The Subject field is required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The Message field is required.")]
        public string Message { get; set; }

        [ValidateNever]
        public IFormFile? Attachment { get; set; }
    }
}
