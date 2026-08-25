
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookly.Data.Models;

namespace Bookly.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public List<Borrow> ActiveBorrows { get; set; } = new();
        public List<Borrow> BorrowHistory { get; set; } = new();

        public int ActiveBorrowsCount => ActiveBorrows.Count;
        public int HistoryCount => BorrowHistory.Count;
        public int FavoritesCount { get; set; }

        public UrgentBorrowDto? UrgentBorrow { get; set; }


        //public int TotalBorrowedCount => ActiveBorrows.Count;
    }

    public class UrgentBorrowDto
    {
        public string BookTitle { get; set; } = string.Empty;
        public DateTime ReturnDeadline { get; set; }
    }
}
