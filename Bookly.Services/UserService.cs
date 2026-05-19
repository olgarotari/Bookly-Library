using Bookly.Data.Models;
using Bookly.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> SearchUsersAsync(string term)
        {
            return await _userRepository.SearchUsersAsync(term);
        }

        public ApplicationUser GetUserByEmailAndPass(string email,  string password)
        {
            return _userRepository.GetByEmailAndPass(email, password);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _userRepository.Update(user);
            await _userRepository.SaveAsync();
        }

        public async Task RegisterUserAsync(string fullName, string email, string password)
        {
            var newUser = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                Password = password,
                RoleId = 2
            };

            _userRepository.Add(newUser);
            await _userRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _userRepository.Delete(user);
                await _userRepository.SaveAsync();
            }
        }
    }
}
