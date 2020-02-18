﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TinyClothes.Data;
using TinyClothes.Models;

namespace TinyClothes.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreContext _context;

        public AccountController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                // if username is not taken
                if (!await AccountDb.IsUsernameTaken(reg.Username, _context))
                {
                    Account acc = new Account()
                    {
                        Email = reg.Email,
                        FullName = reg.FullName,
                        Username = reg.Username,
                        Password = reg.Password
                    };

                    // Add account to DB
                    await AccountDb.Register(_context, acc);
                    return RedirectToAction("Index", "Home");
                }
                else // If username is taken, add error
                {
                    // display error with other username error messages
                    ModelState.AddModelError(nameof(Account.Username), "Username already taken");
                }
            }
            return View(reg);
        }
    }
}