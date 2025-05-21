using System.ComponentModel.DataAnnotations;

namespace BAI5_CONGD.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(150)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Tác giả không được để trống")]
        public string? Author { get; set; }

        [Range(1, 999999, ErrorMessage = "Giá trị từ 1 - 999999")]
        public decimal Price { get; set; }

        [StringLength(100)]
        public string? Image { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Chưa chọn thể loại")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
