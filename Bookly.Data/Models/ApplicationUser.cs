using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Bookly.Data.Models
{
    public class ApplicationUser
    {
        public  int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public int RoleId { get; set; }

        public string Password { get; set; }
        public ApplicationRole Role { get; set; }

        public List<Borrow> Borrows { get; set; } = new List<Borrow>();

    }
}
