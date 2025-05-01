using Readzy.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Readzy.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
       
        [Required]
        public string Author { get; set; }
       
        [Required]
        [Range(1,1000)]
        public double Price { get; set; }

        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Description { get; set; }


        [Required]
        [RegularExpression(@"^(?!0+$)\d+$" , ErrorMessage ="Please, Select Category")]
        public int CategoryId { get; set; }
       
        [Required]
        public IFormFile Photo { get; set; }

        public string? ImageURL { get; set; }
        public List<Category>? categories { get; set; }
        public List<Product>? products { get; set; }


    }
}
