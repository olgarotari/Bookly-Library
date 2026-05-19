using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bookly.Data.Models
{
    public class ApplicationRole
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
