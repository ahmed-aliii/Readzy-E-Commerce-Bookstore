using System.ComponentModel.DataAnnotations;

namespace Readzy.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        //Navigational Prop
        public virtual ICollection<Product>? products { get; set; }
    }
}
