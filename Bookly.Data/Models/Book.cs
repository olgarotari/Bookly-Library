using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title  { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        public int Quantity { get; set; }

        public virtual Author Author { get; set; }
        public virtual Category Category {  get; set; }
        public List<Borrow> Borrows { get; set; } = new List<Borrow>(); 
        public bool IsBorrowed { get; set; } = false;

        public string Summary { get; set; }
        public string? ImageUrl { get; set; }
    }
}
