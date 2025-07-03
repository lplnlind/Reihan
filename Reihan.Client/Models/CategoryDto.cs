using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reihan.Client.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا نام دسته بندی را بنویسید")]
        public string Name { get; set; } = string.Empty;
        public int? ProductsInThisCategory { get; set; }
    }
}
