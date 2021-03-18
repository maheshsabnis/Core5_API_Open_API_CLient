using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBaseAuthentication.Models
{
    public class Product
    {
        [Key]
        public int ProductRowId { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Product Price is required")]
        public int Price { get; set; }
    }
}
