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
        private readonly IBookCopyRepository _bookCopyRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        public BookService(IBookRepository bookRepository, IBookCopyRepository bookCopyRepository, IFavoriteRepository favoriteRepository)
        {
            _bookRepository = bookRepository;
            _bookCopyRepository = bookCopyRepository;
            _favoriteRepository = favoriteRepository;
        }

        //Get All
        public async Task<List<BookViewModel>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return books.Select(b =>
            { 
                int available = b.BookCopies != null ? b.BookCopies.Count(copy => copy.IsAvailable) : 0;
                int total = b.Quantity;

                return new BookViewModel
                {


                    Id = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.Name,
                    CategoryId = b.CategoryId,
                    CategoryName = b.Category.Name,
                    Quantity = available,
                    QuantityDisplay = $"{available} / {total}",
                    Summary = b.Summary,
                    ImageUrl = b.ImageUrl,
                };

            }).ToList();
        }

        //Get by Id
        public async Task<BookViewModel?> GetByIdAsync(int id, int userId)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
                return null;

            var isFavorite = await _favoriteRepository.IsFavoriteAsync(userId, id);

            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.Name,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name,
                Quantity = book.Quantity,
               // IsBorrowed = book.IsBorrowed,
                Summary = book.Summary,
                ImageUrl = book.ImageUrl,
                IsFavorite = isFavorite,
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
                //IsBorrowed = model.IsBorrowed,
                Summary = model.Summary,
                ImageUrl = model.ImageUrl
            };
            _bookRepository.AddBook(book);
            _bookRepository.SaveAsync().Wait();

            for (int i = 0; i < model.Quantity; i++)
            {
                var newCopy = new BookCopy
                {
                    BookId = book.Id,
                    IsAvailable = true,
                    InventoryNumber = $"INV-{book.Id}-{i + 1}"
                    
                };

                _bookCopyRepository.UpdateAsync(newCopy).Wait();
            }

            _bookRepository.SaveAsync().Wait();
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
            //book.IsBorrowed = model.IsBorrowed;
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

        //Live search
        public async Task<IEnumerable<Book>> LiveSearchBookAsync(string term)
        {
            return await _bookRepository.LiveSearchBookAsync(term);
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
