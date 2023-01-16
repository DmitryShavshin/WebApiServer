using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiServer.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(70)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string ImgUrl { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        [ForeignKey("Brand")]
        public Guid BrandId { get; set; }
        [ValidateNever]
        public Brand Brand { get; set; }
        [ValidateNever]
        [NotMapped]
        public IEnumerable<CategoryProduct> CategoryProducts { get; set; }
    }
}
