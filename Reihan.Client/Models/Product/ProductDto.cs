using System.ComponentModel.DataAnnotations;

namespace Reihan.Client.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="مقدار فیلد را به درستی وارد کنید")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage ="مقدار فیلد را به درستی وارد کنید")]
        public decimal Price { get; set; }

        [Required(ErrorMessage ="مقدار فیلد را به درستی وارد کنید")]
        public int StockQuantity { get; set; }

        public string Description { get; set; } = string.Empty;
         
        [Required(ErrorMessage ="مقدار فیلد را به درستی وارد کنید")]
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}
