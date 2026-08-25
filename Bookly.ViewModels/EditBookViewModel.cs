using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.ViewModels
{
    public class EditBookViewModel 
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Title is required!")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Author selection is required.")]
        public int AuthorId { get; set; }


        [Required(ErrorMessage = "Category selection is required.")]
        public int CategoryId { get; set; }


        [Range(0, 1000)]
        public int Quantity { get; set; }

        public string QuantityDisplay { get; set; }

        public bool IsBorrowed { get; set; }

        public string Summary { get; set; }
        public string? ImageUrl { get; set; }
    }
}
