using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Readzy.DataAccess.Repository.CategoryRepository;
using Readzy.DataAccess.Repository.ProductRepository;
using Readzy.Models.Entities;
using Readzy.Utility;
using Readzy.ViewModels;

namespace Readzy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            IProductRepository productRepository , 
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            ProductViewModel productVM
                = new ProductViewModel()
                {
                    products = productRepository.GetAll().ToList(),
                    categories = categoryRepository.GetAll().ToList()
                };
            return View("Index", productVM);
        }


        //#region Create Product
        ////Product/Create
        ////Category/Create => To Open Form Page
        //public IActionResult Create()
        //{
        //    ProductViewModel productVM = new ProductViewModel()
        //    {
        //        categories = categoryRepository.GetAll().ToList()
        //    };

        //    //Category category = new Category();
        //    return View("Create", productVM);
        //}


        //[HttpPost]
        //public IActionResult SaveCreate(ProductViewModel productVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        #region Photo
        //        string uniqueFileName = null;

        //        if (productVM.Photo != null)
        //        {
        //            //check 
        //            //Combine wwwroot path with Images folder path
        //            string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

        //            //Guid (Global Unique Identifier) to create unique file name
        //            uniqueFileName = Guid.NewGuid().ToString() + "_" + productVM.Photo.FileName;

        //            //Combine images folder path with unique file name
        //            string filePath = Path.Combine(imagesFolderPath, uniqueFileName);

        //            //Save A copy of the file using IFormFile CopyTo method To the spcified path
        //            productVM.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
        //        }

        //        #endregion

        //        //Mapping
        //        Product newProduct = new Product()
        //        {
        //            ImageURL = uniqueFileName,
        //            Title = productVM.Title,
        //            Author = productVM.Author,
        //            Price = productVM.Price,
        //            ISBN = productVM.ISBN,
        //            Description = productVM.Description,
        //            CategoryId = productVM.CategoryId,
        //        };

        //        productRepository.Add(newProduct);
        //        productRepository.Save();

        //        TempData["success"] = "Book Created Successfully"; //For Notification Messege

        //        return RedirectToAction("Index");
        //    }

        //    //Refill 
        //    productVM.categories = categoryRepository.GetAll().ToList();
        //    return View("Create", productVM);
        //}
        //#endregion


        //#region  Edit/Update Product
        //public IActionResult Edit(int? id)
        //{
        //    Product product = productRepository.GetById(id);
        //    ProductViewModel productVM =
        //         new ProductViewModel()
        //         {
        //             Id = product.Id,
        //             Title = product.Title,
        //             Author = product.Author,
        //             Price = product.Price,
        //             ISBN = product.ISBN,
        //             Description = product.Description,
        //             CategoryId = product.CategoryId,
        //             categories = categoryRepository.GetAll().ToList(),
        //         };

        //    return View("Edit", productVM);
        //}


        //[HttpPost]
        //public IActionResult SaveEdit(ProductViewModel productVM)
        //{
        //    Product targetedProduct = 
        //        productRepository.GetById(productVM.Id);

        //    if (ModelState.IsValid && targetedProduct != null) //Valid
        //    {

        //        #region Photo
        //        string uniqueFileName = null;

        //        if (productVM.Photo != null)
        //        {
        //            //check 
        //            //Combine wwwroot path with Images folder path
        //            string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

        //            //Guid (Global Unique Identifier) to create unique file name
        //            uniqueFileName = Guid.NewGuid().ToString() + "_" + productVM.Photo.FileName;

        //            //Combine images folder path with unique file name
        //            string filePath = Path.Combine(imagesFolderPath, uniqueFileName);

        //            //Save A copy of the file using IFormFile CopyTo method To the spcified path
        //            productVM.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
        //        }

        //        #endregion

        //        targetedProduct.Title = productVM.Title;
        //        targetedProduct.Author = productVM.Author;
        //        targetedProduct.Price = productVM.Price;
        //        targetedProduct.ISBN = productVM.ISBN;
        //        targetedProduct.Description = productVM.Description;
        //        targetedProduct.CategoryId = productVM.CategoryId;
        //        targetedProduct.ImageURL = uniqueFileName;

        //        categoryRepository.Save();

        //        TempData["success"] = "Book Updated Successfully";

        //        return RedirectToAction("Index");
        //    }

        //    return View("Edit", productVM);
        //}


        //#endregion



        #region Create/Update Product

        public IActionResult UpSert(int? id) //Update+Insert
        {
            //Create
            if (id == 0 || id == null) //Create
            {
                ProductViewModel CreateproductVM = new ProductViewModel()
                {
                    categories = categoryRepository.GetAll().ToList()
                };

                //Category category = new Category();
                return View("UpSert", CreateproductVM);
            }
            //Update
            else
            {
                Product product = productRepository.GetById(id);
                ProductViewModel productVM =
                     new ProductViewModel()
                     {
                         Id = product.Id,
                         Title = product.Title,
                         Author = product.Author,
                         Price = product.Price,
                         ISBN = product.ISBN,
                         Description = product.Description,
                         CategoryId = product.CategoryId,
                         categories = categoryRepository.GetAll().ToList(),
                         ImageURL = product.ImageURL,
                     };

                return View("UpSert", productVM);
            }

        }


        [HttpPost]
        public IActionResult SaveUpSert(ProductViewModel productVM)
        {

            #region Photo
            string uniqueFileName = null;

            if (productVM.Photo != null)
            {
                //check 
                //Combine wwwroot path with Images folder path
                string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

                //Guid (Global Unique Identifier) to create unique file name
                uniqueFileName = Guid.NewGuid().ToString() + "_" + productVM.Photo.FileName;

                //Combine images folder path with unique file name
                string filePath = Path.Combine(imagesFolderPath, uniqueFileName);

                if (!string.IsNullOrEmpty(productVM.ImageURL))
                {
                    var oldImagePath =
                        Path.Combine(imagesFolderPath, productVM.ImageURL.TrimStart('\\'));
                    //Delete Old Image
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                //Save A copy of the file using IFormFile CopyTo method To the spcified path
                productVM.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            #endregion

            //Create Product
            if (productVM.Id == 0 || productVM.Id == null) 
            {

                if (ModelState.IsValid)
                {
                    //Mapping
                    Product newProduct = new Product()
                    {
                        ImageURL = uniqueFileName,
                        Title = productVM.Title,
                        Author = productVM.Author,
                        Price = productVM.Price,
                        ISBN = productVM.ISBN,
                        Description = productVM.Description,
                        CategoryId = productVM.CategoryId,
                    };

                    productRepository.Add(newProduct);
                    productRepository.Save();

                    TempData["success"] = "Book Created Successfully"; //For Notification Messege

                    return RedirectToAction("Index");
                }

                //Refill 
                productVM.categories = categoryRepository.GetAll().ToList();
                return View("UpSert", productVM);
            }
            //Update Product
            else
            {
                Product targetedProduct =
               productRepository.GetById(productVM.Id);

                if (ModelState.IsValid && targetedProduct != null) //Valid
                {
                    targetedProduct.Title = productVM.Title;
                    targetedProduct.Author = productVM.Author;
                    targetedProduct.Price = productVM.Price;
                    targetedProduct.ISBN = productVM.ISBN;
                    targetedProduct.Description = productVM.Description;
                    targetedProduct.CategoryId = productVM.CategoryId;
                    targetedProduct.ImageURL = uniqueFileName;

                    categoryRepository.Save();

                    TempData["success"] = "Book Updated Successfully";

                    return RedirectToAction("Index");
                }
                return View("UpSert", productVM);
            }
        }
        #endregion



        #region Delete Product
        public IActionResult Delete(int? id)
        {
            Product product =
                    productRepository.GetById(id);

            if (product != null || id != 0 || id != null)
            {
                return View("Delete", product);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult SaveDelete(int? id)
        {
            Product product =
                    productRepository.GetById(id);

            if (product != null || id != 0 || id != null)
            {

                productRepository.Remove(product);
                productRepository.Save();

                TempData["success"] = "Category Deleted Successfully";

                return RedirectToAction("Index");
            }

            return NotFound();
        }
        #endregion



        #region DataTables API
        public IActionResult GetAllProducts()
        {

            var products = productRepository.GetAll().ToList();
            var categories = categoryRepository.GetAll().ToList();

            var data = products.Select(p => new
            {
                p.Id,
                p.Title,
                p.Author,
                p.Price,
                Category = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.Name
            });

            return Json(new { data });
        }
        #endregion

    }
}
