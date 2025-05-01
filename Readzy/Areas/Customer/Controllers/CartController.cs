using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readzy.DataAccess.Repository.ApplicationUserRepository;
using Readzy.DataAccess.Repository.OrderDetailRepository;
using Readzy.DataAccess.Repository.OrderHeaderRepository;
using Readzy.DataAccess.Repository.ShoppingCartRepository;
using Readzy.Models.Entities;
using Readzy.Utility;
using Readzy.ViewModels;
using Stripe.Checkout;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Readzy.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IOrderHeaderRepository orderHeaderRepository;
        private readonly IOrderDetailRepository orderDetailRepository;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        public CartController(
            IShoppingCartRepository shoppingCartRepository , 
            IApplicationUserRepository applicationUserRepository,
            IOrderHeaderRepository orderHeaderRepository,
            IOrderDetailRepository orderDetailRepository
            )
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.orderHeaderRepository = orderHeaderRepository;
            this.orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            //Get User Id
            var identityClaims = (ClaimsIdentity)User.Identity;
            var userId = identityClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartViewModel()
            {
                ShoppingCartList = shoppingCartRepository.GetAllByUserIdWithIncludeProduct(userId),
                OrderHeader = new OrderHeader()
            };

            //Calculate Order Total
            double orderTotal = 0;
            if (ShoppingCartVM.ShoppingCartList != null)
            {
                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    cart.Price = cart.Product.Price; //Get the price of every product 
                    orderTotal += cart.Count * cart.Product.Price;
                }
            }
            ShoppingCartVM.OrderHeader.OrderTotal = orderTotal;


            return View(ShoppingCartVM);
        }

        //----------Order Summary ----------

        public IActionResult Summary()
        {
            //Get User Id
            var identityClaims = (ClaimsIdentity)User.Identity;
            var userId = identityClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartViewModel()
            {
                ShoppingCartList = shoppingCartRepository.GetAllByUserIdWithIncludeProduct(userId),
                OrderHeader = new OrderHeader()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = applicationUserRepository.GetById(userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;


            //Calculate Order Total
            double orderTotal = 0;
            if (ShoppingCartVM.ShoppingCartList != null)
            {
                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    cart.Price = cart.Product.Price; //Get the price of every product 
                    orderTotal += cart.Count * cart.Product.Price;
                }
            }
            ShoppingCartVM.OrderHeader.OrderTotal = orderTotal;


            return View(ShoppingCartVM);
        }


        [HttpPost]
        public IActionResult SummaryPost()
        {
            //Get User Id
            var identityClaims = (ClaimsIdentity)User.Identity;
            var userId = identityClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = shoppingCartRepository.GetAllByUserIdWithIncludeProduct(userId);
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ShoppingCartVM.OrderHeader.ApplicationUser = applicationUserRepository.GetById(userId);

            //Calculate Order Total
            double orderTotal = 0;
            if (ShoppingCartVM.ShoppingCartList != null)
            {
                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    cart.Price = cart.Product.Price; //Get the price of every product 
                    orderTotal += cart.Count * cart.Product.Price;
                }
            }
            ShoppingCartVM.OrderHeader.OrderTotal = orderTotal;

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

            //Create Order Header
            orderHeaderRepository.Add(ShoppingCartVM.OrderHeader);
            orderHeaderRepository.Save();

            //Create Order Details
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count,
                };
                orderDetailRepository.Add(orderDetail);
                orderDetailRepository.Save();
            }

            //------- Stripe Payment Logic -----------------------------------
            #region Stripe Payment Logic
            var domain = "https://localhost:44345/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + "Customer/Cart/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                var sessionLineItemOptions = new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(cart.Price * 100),
                        Currency = "usd",
                        ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = cart.Product.Title,
                        },
                    },
                    Quantity = cart.Count,
                };
                options.LineItems.Add(sessionLineItemOptions);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            orderHeaderRepository.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            orderHeaderRepository.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            #endregion


            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }



        //----------Order Confirmation ----------
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader =  orderHeaderRepository.GetByIdWithInclude(id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                orderHeaderRepository.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                orderHeaderRepository.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                orderHeaderRepository.Save();
            }
            
            List<ShoppingCart> shoppingCarts = 
                shoppingCartRepository.GetAllByUserIdWithIncludeProduct(orderHeader.ApplicationUserId).ToList();
            shoppingCartRepository.RemoveRange(shoppingCarts);
            shoppingCartRepository.Save();

            return View(id);
        }





        //----------Plus Count - Minus Count - Remove Order ----------
        public IActionResult Plus(int cartId)
        {
            var cart = shoppingCartRepository.GetById(cartId);
            cart.Count += 1;
            shoppingCartRepository.Update(cart);
            shoppingCartRepository.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = shoppingCartRepository.GetById(cartId);
            if (cart.Count <= 1)
            {
                shoppingCartRepository.Remove(cart);
            }
            else
            {
                cart.Count -= 1;
                shoppingCartRepository.Update(cart);
            }
            shoppingCartRepository.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = shoppingCartRepository.GetById(cartId);
            shoppingCartRepository.Remove(cart);
            shoppingCartRepository.Save();
            return RedirectToAction(nameof(Index));
        }


    }
}
