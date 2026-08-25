using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get ; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Status { get; set; } = "Pending"; //"Pending", "ReadyForPickup", "Expired", "Completed"
    }
}
