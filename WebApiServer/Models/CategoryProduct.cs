using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApiServer.Models;

namespace WebApiServer.Models
{
    [Keyless]
    public class CategoryProduct
    {
        [JsonIgnore]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        [JsonIgnore]
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}