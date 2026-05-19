using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.ViewModels
{
    public class BookViewModel : EditBookViewModel
    {
     
        public string AuthorName { get; set; }

        public string CategoryName {  get; set; }


    }
}
