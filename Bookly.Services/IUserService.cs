using Bookly.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string term);
        ApplicationUser GetUserByEmailAndPass(string email, string password);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(ApplicationUser user);
        Task RegisterUserAsync(string fullName, string email, string password);
        Task DeleteAsync(int id);
    }
}
