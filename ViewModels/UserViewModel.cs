using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlleyCatBarbers.ViewModels
{
    public class UserViewModel
    {
       
        [BindNever]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        
        [Display(Name = "Roles")]
        public string Roles { get; set; }



    }
}
