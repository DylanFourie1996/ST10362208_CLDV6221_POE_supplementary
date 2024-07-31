using System.ComponentModel.DataAnnotations;

namespace ST10362208_CLDV6221_POE.REDO.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public string Creator { get; set; }

        [Required]
        public DateTime? DateCreated { get; set; }

        public Guid? ProductKey { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
