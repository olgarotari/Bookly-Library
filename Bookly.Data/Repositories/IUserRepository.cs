using Bookly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Data.Repositories
{
    public interface IUserRepository
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string term);
        ApplicationUser GetByEmailAndPass(string email, string password);

        Task<ApplicationUser> GetByIdAsync(int id);
       
        void Add(ApplicationUser newUser);

        void Delete(ApplicationUser user);

        void Update(ApplicationUser user);
        Task SaveAsync();
        
    }
}
