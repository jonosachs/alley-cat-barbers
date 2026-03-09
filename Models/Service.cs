using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace AlleyCatBarbers.Models
{
    public class Service
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<Booking> Bookings { get; set; }

        public Service()
        {
            Date = DateTime.UtcNow.Date;
            Bookings = new List<Booking>();
        }
    }
}
