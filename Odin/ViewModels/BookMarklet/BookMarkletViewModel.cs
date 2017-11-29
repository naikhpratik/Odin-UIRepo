using System.Collections.Generic;

namespace Odin.ViewModels.BookMarklet
{
    public class BookMarkletViewModel
    {
        public BookMarkletViewModel()
        {
            Orders = new List<BookMarkletOrderViewModel>();
        }

        public IEnumerable<BookMarkletOrderViewModel> Orders { get; set;  }
        public string PropertyUrl { get; set; }
    }
}