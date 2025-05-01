using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readzy.DataAccess.Data;
using Readzy.DataAccess.Repository.CategoryRepository;
using Readzy.Models.Entities;
using Readzy.Utility;
using Readzy.ViewModels;

namespace Readzy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = categoryRepository.GetAll().ToList();
            return View("Index", categoryList);
        }


        #region Create Category

        //Category/Create => To Open Form Page
        public IActionResult Create()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            //Category category = new Category();
            return View("Create", categoryViewModel);
        }

        //Category/SaveCreate
        [HttpPost]
        public IActionResult SaveCreate(CategoryViewModel categoryViewModel)
        {
            if (ModelState.IsValid) //Form Model Is Valid
            {
                Category category = new Category()
                {
                    Name = categoryViewModel.Name,
                    DisplayOrder = categoryViewModel.DisplayOrder,
                };
                categoryRepository.Add(category);
                categoryRepository.Save();

                TempData["success"] = "Category Created Successfully"; //For Notification Message

                return RedirectToAction("Index");
            }

            //Form Model NOT Valid
            return View("Create", categoryViewModel);
        }
      
        #endregion


        #region Edit/Update Category

        // Category/Edit 
        public IActionResult Edit(int? id)
        {
            Category category = categoryRepository.GetById(id);
            CategoryViewModel categoryViewModel = new CategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
            };

            if (id == null || id == 0 || category == null)
            {
                return NotFound();
            }


            return View("Edit", categoryViewModel);
        }


        //Category/SaveEdit
        [HttpPost]
        public IActionResult SaveEdit(CategoryViewModel categoryViewModel)
        {
             Category targetedCategory = 
                categoryRepository.GetById(categoryViewModel.Id);
            
            if (ModelState.IsValid && targetedCategory != null) //Valid
            {
                targetedCategory.Name = categoryViewModel.Name;
                targetedCategory.DisplayOrder = categoryViewModel.DisplayOrder;
                categoryRepository.Save();

                TempData["success"] = "Category Updated Successfully";

                return RedirectToAction("Index");
            }

            return View("Edit", categoryViewModel);
        }
        #endregion


        #region Delete Category

        public IActionResult Delete(int? id)
        {
            Category category =
                    categoryRepository.GetById(id);

            if (category != null || id != 0 || id != null)
            {
                return View("Delete", category);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult SaveDelete(int? id)
        {
            Category category =
                    categoryRepository.GetById(id);
            
            if (category != null || id != 0 || id != null)
            {

                categoryRepository.Remove(category);
                categoryRepository.Save();

                TempData["success"] = "Category Deleted Successfully";

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        #endregion


        #region Category Custom Validators

        public IActionResult IsSameAsName(int displayOrder, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json("Input is Required");
            }
            else if (displayOrder.ToString().ToLower() == name.ToLower())
            {
                return Json("Display Order & Name Can not be the same");
            }

            return Json(true);
        }

        public IActionResult Unique(int displayOrder, int Id)
        {
            Category category =categoryRepository.IsUnique(displayOrder , Id);

            if (category == null)
            {
                return Json(true); // Unique
            }

            return Json($"Display Order {displayOrder} is already in use."); // ❌ Duplicate
        }
        #endregion

    }
}
