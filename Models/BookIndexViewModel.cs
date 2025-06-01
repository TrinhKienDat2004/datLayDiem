using System.Collections.Generic;

namespace BAI5_CONGD.Models
{
    public class BookIndexViewModel
    {
        public List<Category>? Categories { get; set; }
        public List<Book>? Books { get; set; }
        public int? SelectedCategoryId { get; set; }

        public string? SearchString { get; set; }
    }
}
