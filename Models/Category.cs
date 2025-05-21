namespace BAI5_CONGD.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}
