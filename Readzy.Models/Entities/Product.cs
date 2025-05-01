using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author{ get; set; }
        public string ImageURL { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; } //Navigational Property
    }
}
