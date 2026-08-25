using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Models
{
    public class Borrow
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int BookCopyId { get; set; }
        public BookCopy BookCopy { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public DateTime DueDate { get; set; } // termen limita de imprumut

        public string Status { get; set; } = "Active";
        public decimal FineAmount { get; set; } = 0.00m; //penalizre intarzieri
        
    }
}
