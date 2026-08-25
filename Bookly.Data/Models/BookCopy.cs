using System.ComponentModel.DataAnnotations;
namespace Bookly.Data.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        public string InventoryNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Condition { get; set; } = "New"; //starea cartii la returnare
        public List<Borrow> Borrows { get; set; } = new List<Borrow>();
    }
}
