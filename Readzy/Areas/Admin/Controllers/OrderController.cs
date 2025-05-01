using Microsoft.AspNetCore.Mvc;
using Readzy.DataAccess.Repository.OrderHeaderRepository;
using Readzy.Models.Entities;

namespace Readzy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository orderHeaderRepository;

        public OrderController(IOrderHeaderRepository orderHeaderRepository)
        {
            this.orderHeaderRepository = orderHeaderRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region DataTables API
        public IActionResult GetAllOrders()
        {
            List<OrderHeader> orderHeaders = orderHeaderRepository.GetByAllWithInclude().ToList();

            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
