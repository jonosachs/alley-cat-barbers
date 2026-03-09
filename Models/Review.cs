using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AlleyCatBarbers.Models
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int Rating { get; set; }

        public string Comments { get; set; }
        
    }
}
