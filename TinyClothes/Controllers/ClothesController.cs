using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class ClothesController : Controller
    {
        private readonly StoreContext _context;
        public ClothesController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ShowAll(int? page)
        {
            const int PageSize = 2;
            // (coalescing operator) if page has value, use value, otherwise use 1
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            int pageNumber = page ?? 1;
            ViewData["CurrentPage"] = pageNumber;

            int maxPage = await GetMaxPage(PageSize);
            ViewData["MaxPage"] = maxPage;

            List<Clothing> clothes = await ClothingDb.GetClothingByPage(_context, pageNum: pageNumber, pageSize: PageSize);
            return View(clothes);
        }

        private async Task<int> GetMaxPage(int PageSize)
        {
            int numProducts = await ClothingDb.GetNumClothing(_context) / PageSize;

            // round up always, no partial page number (2.1 pages = 3 pages)
            int maxPage = Convert.ToInt32(Math.Ceiling((double)numProducts / PageSize));
            return maxPage;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Clothing c)
        {
            if (ModelState.IsValid)
            {
                await ClothingDb.Add(c, _context);
                // TempData lasts for one redirect
                TempData["Message"] = $"{c.Title} added successfully";
                return RedirectToAction("ShowAll");
            }

            // Return same view with error/validation messages
            return View(c);

            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            if (c == null) // Clothing not in DB
            {
                return NotFound();  // Returns a HTTP 404 - Not Found
            }
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Clothing c)
        {
            if (ModelState.IsValid)
            {
                await ClothingDb.Edit(c, _context);
                ViewData["Message"] = c.Title + " updated successfully";
                return View(c);
            }
            return View(c);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            if (c == null) // If clothing does not exist
            {
                return NotFound();
            }
            return View(c);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            await ClothingDb.Delete(c, _context);
            TempData["Message"] = $"{c.Title} Clothing deleted successfully";
            return RedirectToAction(nameof(ShowAll));
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchCriteria search)
        {
            if (ModelState.IsValid)
            {
                // Prepare query - SELECT * FROM Clothes
                // Does not get sent to DB
                IQueryable<Clothing> allClothes = (from c in _context.Clothing
                                               select c);

                if (search.MinPrice.HasValue)
                {   // WHERE Price > MinPrice
                    allClothes = (from c in allClothes
                                  where c.Price >= search.MinPrice
                                  select c);
                }

                if (search.MaxPrice.HasValue)
                {   // WHERE Price < MaxPrice
                    allClothes = (from c in allClothes
                                  where c.Price <= search.MaxPrice
                                  select c);
                }

                if (!string.IsNullOrWhiteSpace(search.Size))
                {   // WHERE Size matches
                    allClothes = (from c in allClothes
                                  where c.Size == search.Size
                                  select c);
                }

                if (!string.IsNullOrWhiteSpace(search.Type))
                {   // WHERE Type matches
                    allClothes = (from c in allClothes
                                  where c.Type == search.Type
                                  select c);
                }

                if (!string.IsNullOrWhiteSpace(search.Title))
                {   // WHERE Title is contained
                    allClothes = (from c in allClothes
                                  where c.Title.Contains(search.Title)
                                  select c);
                }

                search.Results = allClothes.ToList();
                
            }
            return View(search);

        }
    }
}