using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Readzy.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Input must contain letters only. No numbers or special characters allowed.")]
        public string Name { get; set; }

        [Range(minimum: 1, maximum: 100, ErrorMessage = "Display Order must be between 1-100")]
        //[Remote(action:"IsSameAsName" , controller:"Category" , AdditionalFields = "Name")] //Custom Validator
        [Remote(action: "Unique", controller: "Category" , AdditionalFields = "Id")]//Custom Validator
        public int DisplayOrder { get; set; }
    }
}
