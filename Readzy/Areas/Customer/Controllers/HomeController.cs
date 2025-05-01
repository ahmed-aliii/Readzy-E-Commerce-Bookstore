using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readzy.DataAccess.Repository.ProductRepository;
using Readzy.DataAccess.Repository.ShoppingCartRepository;
using Readzy.Models;
using Readzy.Models.Entities;

namespace Readzy.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository productRepository;
    private readonly IShoppingCartRepository shoppingCartRepository;

    public HomeController(
        ILogger<HomeController> logger ,
        IProductRepository productRepository,
        IShoppingCartRepository shoppingCartRepository
        )
    {
        _logger = logger;
        this.productRepository = productRepository;
        this.shoppingCartRepository = shoppingCartRepository;
    }



    public IActionResult Index()
    {
        List<Product> products = new List<Product>();
        products = productRepository.GetAll().ToList();

        return View(products);
        
    }

    //--------------- Shopping Cart ------------------------------------------
    public IActionResult Details(int id)
    {
        Product product = new Product();
        product = productRepository.GetByIdWithInclude(id);

        ShoppingCart shoppingCart = new ShoppingCart()
        {
            Product = product,
            Count = 1,
            ProductId = id,
        };
        return View(shoppingCart);
    }


    [HttpPost]
    [Authorize]
    public IActionResult SaveDetails(ShoppingCart shoppingCart)
    {
        //Get User Id
        var identityClaims = (ClaimsIdentity)User.Identity;
        var userId = identityClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

        shoppingCart.ApplicationUserId = userId;



        ShoppingCart cartFromDb = shoppingCartRepository.GetFirstOrDefault(userId , shoppingCart.ProductId);

        if(cartFromDb != null)
        {
            //cart already exists
            //Update
            cartFromDb.Count += shoppingCart.Count;
            shoppingCartRepository.Update(cartFromDb);
        }
        else
        {
            //cart does not exist
            //Add new Cart
            shoppingCart.ApplicationUserId = userId;
            shoppingCartRepository.Add(shoppingCart);
        }
        TempData["success"] = "Shopping Cart Updated Successfully";
        shoppingCartRepository.Save();


        return RedirectToAction(nameof(Index));
    }





    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
