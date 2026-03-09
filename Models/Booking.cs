using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AlleyCatBarbers.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public TimeOnly TimeSlot { get; set; }
        
        [BindNever]
        public string UserId { get; set; }
        
        [BindNever]
        public ApplicationUser User { get; set; }
        
        public int ServiceId { get; set; }

        [BindNever]
        public Service Service { get; set; }

        
    }
}
