using Bookly.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Data.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly BooklyDbContext _context;

        public UserRepository(BooklyDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await _context.Users.Take(10).ToListAsync();
            }

            return await _context.Users
                .Where(u => u.FullName.Contains(term))
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


        public ApplicationUser GetByEmailAndPass(string email, string password)
        {
            return  _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

        }

        public void Add(ApplicationUser newUser)
        {
            _context.Users.Add(newUser);
        }

        public void Update(ApplicationUser user)
        {
            _context.Users.Update(user);
        }

        public void Delete(ApplicationUser user)
        {
            _context.Users.Remove(user);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
