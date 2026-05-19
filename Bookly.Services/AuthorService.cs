using Bookly.Data.Models;
using Bookly.Data.Repositories;
using Bookly.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<List<Author>> GetAllAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Author>> SearchAuthorsAsync(string term)
        {
            return await _authorRepository.SearchAuthorsAsync(term);
        }

        public async Task CreateAsync(string authorName)
        {
            if (string.IsNullOrWhiteSpace(authorName))
            {
                throw new Exception("Author name is required.");
            }

            if (authorName.Length < 2)
            {
                throw new Exception("Author name is too short. Minimum 2 characters.");
            }
            
            if (authorName.Length > 100)
            {
                throw new Exception("Author name is too long. Maximum 100 characters.");
            }

            var author = new Author
            {
                Name = authorName,
            };
            await _authorRepository.AddAsync(author);
            await _authorRepository.SaveAsync();
             
        }

        public async Task UpdateAsync(int id, string newName)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new Exception("Author not found");
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new Exception("New name cannot be empty");
            }
            author.Name = newName;

            await _authorRepository.Update(author);
            await _authorRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author != null)
            {
                await _authorRepository.Delete(author);
                await _authorRepository.SaveAsync();
            }
        }

    }
}
