using Bookly.Data.Models;
using Bookly.Data.Repositories;
using Bookly.ViewModels;

namespace Bookly.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookRepository _bookRepository;
        public BorrowService(IBorrowRepository borrowRepository, IBookRepository bookRepository)
        {
            _borrowRepository = borrowRepository;
            _bookRepository = bookRepository;
        }

        public async Task<List<BorrowViewModel>> GetAllAsync()
        {
            var borrows = await _borrowRepository.GetAllAsync();

            return borrows.Select(b => new BorrowViewModel
            {
                Id = b.Id,
                UserName = b.User?.FullName ?? "Utilizator necunoscut",
                BookTitle = b.Book?.Title ?? "Titlu indisponibil",
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate

            }).ToList();

        }

        public async Task<BorrowViewModel?> GetByIdAsync(int id)
        {
            var borrow = await _borrowRepository.GetByIdAsync(id);

            if (borrow == null) return null;
            return new BorrowViewModel
            {
                Id = borrow.Id,
                UserId = borrow.UserId,
                UserName = borrow.User.FullName,
                BookId = borrow.BookId,
                BookTitle = borrow.Book.Title,
                BorrowDate = borrow.BorrowDate,
                ReturnDate = borrow.ReturnDate

            };
        }

        public  async Task CreateAsync(EditBorrowViewModel model)
        {
            var borrow = new Borrow
            {
                UserId = model.UserId,
                BookId = model.BookId,
                BorrowDate = model.BorrowDate,
                ReturnDate = model.ReturnDate
            };
            await _borrowRepository.Create(borrow);
            await _borrowRepository.SaveAsync();

        }

        public async Task UpdateAsync(int id, EditBorrowViewModel model)
        {
            var borrow = await _borrowRepository.GetByIdAsync(id);

            if (borrow == null)
                throw new Exception("Borrow is not found");

            borrow.UserId = model.UserId;
            borrow.BookId = model.BookId;
            borrow.BorrowDate = model.BorrowDate;
            borrow.ReturnDate = model.ReturnDate;

            await _borrowRepository.Update(borrow);
            await _borrowRepository.SaveAsync();

        }



        public async Task ReturnBookAsync(int bookId)
        {
            var borrow =  _borrowRepository.GetActiveBorrow(bookId);
            if (borrow == null)
                throw new Exception("Book is not borrowed");
            borrow.ReturnDate = DateTime.UtcNow;

            await  _borrowRepository.Update(borrow);
        }

        public async Task<bool> DeleteBorrowAsync(int id)
        {
            var borrow = await _borrowRepository.GetByIdAsync(id);
            if (borrow == null)
                return false;

            if (borrow.ReturnDate == null)
            {
                var book = await _bookRepository.GetByIdAsync(borrow.BookId);
                if (book != null)
                {
                    book.IsBorrowed = false;
                    _bookRepository.Update(book);
                }
            }

            _borrowRepository.Delete(borrow);
            await _borrowRepository.SaveAsync();

            return true;

            
        }

    }
}
