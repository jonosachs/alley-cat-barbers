namespace AlleyCatBarbers.DTOs
{
    public class UserRecord
    {
        //Email,Password,Role,FirstName,LastName,PhoneNumber,DateOfBirth
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }


    }
}
