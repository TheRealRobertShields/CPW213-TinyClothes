using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class CartController : Controller
    {
        // To read cookie data.
        private readonly StoreContext _context;
        private readonly IHttpContextAccessor _http;

        public CartController(StoreContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        // Display all products in cart.
        public IActionResult Index()
        {
            return View();
        }

        // Add a single product to the shopping cart.
        public async Task<IActionResult> AddToCartAsync(int id, string prevUrl)
        {
            Clothing c = await ClothingDb.GetClothingById(id, _context);
            if (c != null)
            {
                CartHelper.Add(c, _http);
            }
            return Redirect(prevUrl);
        }

        // review order page.
        public IActionResult Checkout() 
        {
            return View();
        }
    }
}