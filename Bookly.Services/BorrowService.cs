using Bookly.Data.Models;
using Bookly.Data.Repositories;
using Bookly.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IBookCopyRepository _bookCopyRepository;
        public BorrowService(IBorrowRepository borrowRepository, IBookCopyRepository bookCopyRepository)
        {
            _borrowRepository = borrowRepository;
            _bookCopyRepository = bookCopyRepository;
            
        }

        public async Task<List<BorrowViewModel>> GetAllAsync()
        {
            var borrows = await _borrowRepository.GetAllAsync();


            return borrows.Select(b => new BorrowViewModel
            {
                Id = b.Id,
                UserName = b.User?.FullName ?? "Utilizator necunoscut",
                BookTitle = b.BookCopy?.Book.Title ?? "Titlu indisponibil",
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                ReturnDate = b.ReturnDate,
                Status = b.Status

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
                BookId = borrow.BookCopy.BookId,
                BookTitle = borrow.BookCopy.Book.Title,
                BorrowDate = borrow.BorrowDate,
                DueDate = borrow.DueDate,
                ReturnDate = borrow.ReturnDate,
                Status = borrow.Status

            };
        }

        public async Task<List<Borrow>> GetActiveBorrowsByUserIdAsync(int userId)
        {
            // Dacă ID-ul nu este valid
            if (userId <= 0)
            {
                return new List<Borrow>();
            }

            var borrows = await _borrowRepository.GetActiveBorrowsByUserIdAsync(userId);

            foreach (var borrow in borrows)
            {
                if (borrow.ReturnDate == null && DateTime.UtcNow > borrow.DueDate)
                {
                    borrow.Status = "Overdue";
                }
            }

            return borrows;


        }

        public async Task<bool> CreateAsync(EditBorrowViewModel model)
        {
            var availableCopy = await _bookCopyRepository.GetAvailableCopyByBookIdAsync(model.BookId);

            if (availableCopy == null)
            {
                return false;
            }

            var borrow = new Borrow
            {
                UserId = model.UserId,
                BookCopyId = availableCopy.Id,
                BorrowDate = model.BorrowDate,
                ReturnDate = null
            };

            availableCopy.IsAvailable = false;
            await _bookCopyRepository.UpdateAsync(availableCopy);

            await _borrowRepository.Create(borrow);
            await _borrowRepository.SaveAsync();

            return true;
        }
       

        public async Task UpdateAsync(int id, EditBorrowViewModel model)
        {
            var borrow = await _borrowRepository.GetByIdAsync(id);

            if (borrow == null)
                throw new Exception("Borrow is not found");

            borrow.UserId = model.UserId;
            borrow.BookCopyId = model.BookId;
            borrow.BorrowDate = model.BorrowDate;
            borrow.ReturnDate = model.ReturnDate;

            await _borrowRepository.Update(borrow);
            await _borrowRepository.SaveAsync();

        }

        public async Task BorrowBookAsync(int userId, int bookId)
        {
            var activeBorrows = await _borrowRepository.GetActiveBorrowsByUserIdAsync(userId);
            if (activeBorrows != null && activeBorrows.Count >= 3)
            {
                throw new InvalidOperationException("Ai atins limita maximă de 3 cărți împrumutate simultan.");
            }

            var availableCopy = await _borrowRepository.GetFirstAvailableCopyAsync(bookId);

            if (availableCopy == null)
            {
                throw new Exception("Nu mai există nicio copie disponibilă pentru această carte!");
            }

            var borrow = new Borrow
            {
                UserId = userId,
                BookCopyId = availableCopy.Id,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14),
                ReturnDate = null
            };

            availableCopy.IsAvailable = false;

            await _borrowRepository.AddAsync(borrow);
        }

        public async Task<string> ReturnBookAsync(int borrowId)
        {
            var borrow = await _borrowRepository.GetActiveBorrowByIdAsync(borrowId);

            if (borrow == null || borrow.ReturnDate != null)
            {
                throw new InvalidOperationException("Împrumutul nu a fost găsit sau cartea a fost deja returnată.");
            }

            var now = DateTime.UtcNow;
            borrow.ReturnDate = now;
            borrow.Status = "Returned";

            if (borrow.BookCopy != null)
            {
                borrow.BookCopy.IsAvailable = true;
            }

            await _borrowRepository.Update(borrow);
            await _borrowRepository.SaveAsync();

            if (now > borrow.DueDate)
            {
                return "Carte returnată cu succes (a depășit termenul limită).";
            }

            return "Carte returnată cu succes la timp!";
        }


        public async Task<List<Borrow>> GetBorrowHistoryByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                return new List<Borrow>();
            }

            return await _borrowRepository.GetBorrowHistoryByUserIdAsync(userId);
        }

        public async Task<bool> ReturnBorrowAsync(int id)
        {
            var borrow = await _borrowRepository.GetByIdAsync(id);
            if (borrow == null || borrow.ReturnDate != null)
            {
                return false;
            }

            borrow.ReturnDate = DateTime.Now;
            borrow.Status = "Returned";

            await _borrowRepository.Update(borrow);
            return true;
        }



        public async Task<bool> DeleteBorrowAsync(int id)
        {
            var borrow = await _borrowRepository.GetByIdAsync(id);
            if (borrow == null)
                return false;

            if (borrow.ReturnDate == null)
            {
                var bookCopy = await _bookCopyRepository.GetByIdAsync(borrow.BookCopyId);
                if (bookCopy != null)
                {
                    bookCopy.IsAvailable = true;
                    await _bookCopyRepository.UpdateAsync(bookCopy);
                    

                }
            }

            _borrowRepository.Delete(borrow);
            await _borrowRepository.SaveAsync();

            return true;

            
        }

    }
}
