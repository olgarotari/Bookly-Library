using Bookly.Data.Models;
using Bookly.Data.Repositories;
using Bookly.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookly.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //Get All
        public async Task<List<BookViewModel>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();

            return books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                AuthorId = b.AuthorId,
                AuthorName = b.Author.Name,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                //AuthorName = b.Author?.Name ?? "Fara autor", 
                //CategoryName = b.Category?.Name ?? "Fara Categorie",
                Quantity = b.Quantity,
                IsBorrowed = b.IsBorrowed,
                Summary = b.Summary,
                ImageUrl = b.ImageUrl

            }).ToList();
        }

        //Get by Id
        public async Task<BookViewModel?> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                return null;

            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.Name,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name,
                Quantity = book.Quantity,
                IsBorrowed = book.IsBorrowed,
                Summary = book.Summary,
                ImageUrl = book.ImageUrl
            };

        }

        //Create
        public void Create(EditBookViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                throw new Exception("Book title is required.");

            if (model.Quantity < 0)
                throw new Exception("Quantity cannot be less than 0.");

            var book = new Book
            {
                Title = model.Title,
                AuthorId = model.AuthorId,
                CategoryId = model.CategoryId,
                Quantity = model.Quantity,
                IsBorrowed = model.IsBorrowed,
                Summary = model.Summary,
                ImageUrl = model.ImageUrl
            };
            _bookRepository.AddBook(book);
        }

        //Update
        public async Task<bool> UpdateAsync(int id, EditBookViewModel model)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null ) 
                return false;

            if (string.IsNullOrWhiteSpace(model.Title))
                throw new Exception("Title cannot be empty during update");

            book.Title = model.Title;
            book.AuthorId = model.AuthorId;
            book.CategoryId = model.CategoryId;
            book.Quantity = model.Quantity;
            book.IsBorrowed = model.IsBorrowed;
            book.Summary = model.Summary;
            book.ImageUrl = model.ImageUrl;

             _bookRepository.Update(book);
            await _bookRepository.SaveAsync();


            return true;
        }

        //Search by

        public async Task<List<Book>> SearchBooksAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return new List<Book>();

            return await _bookRepository.SearchByTermAsync(term);
        }

        //Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                return false;

             _bookRepository.Delete(book);
            await _bookRepository.SaveAsync();

            return true;
        }
    }
}
