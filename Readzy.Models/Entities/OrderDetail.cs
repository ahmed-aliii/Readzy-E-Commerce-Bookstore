using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readzy.Models.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("OrderHeader")]
        public int OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }



        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }


        public int Count { get; set; }
        public double Price { get; set; }

    }
}
