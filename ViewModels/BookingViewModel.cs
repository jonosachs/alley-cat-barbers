using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace AlleyCatBarbers.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public string TimeSlot { get; set; }
        
        [Required]
        public int ServiceId { get; set; }
        
        [ValidateNever]
        public string UserId { get; set; }

    }
}
