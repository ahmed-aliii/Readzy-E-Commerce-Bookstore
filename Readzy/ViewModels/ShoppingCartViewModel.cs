using Readzy.Models.Entities;

namespace Readzy.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

        public OrderHeader OrderHeader { get; set; }
        //public double OrderTotal { get; set; }

    }
}
