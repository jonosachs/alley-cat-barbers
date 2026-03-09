using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlleyCatBarbers.ViewModels
{
    public class ReviewViewModel
    {
        [BindNever]
        public string Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comments { get; set; }

        public DateTime DateCreated { get; set; }

        [BindNever]
        public string FirstName { get; set; }

    }
}
