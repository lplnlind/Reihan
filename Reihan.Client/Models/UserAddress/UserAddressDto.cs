using System.ComponentModel.DataAnnotations;

namespace Reihan.Client.Models
{
    public class UserAddressDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا عنوان آدرس را بنویسید")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "لطفا نام استان را بنویسید")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "لطفا آدرس را بنویسید")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "لطفا نام شهر را بنویسید")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "لطفا کدپستی را بنویسید")]
        public string ZipCode { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }
}
