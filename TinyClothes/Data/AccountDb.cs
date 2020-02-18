using System;
using System.Threading.Tasks;
using TinyClothes.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TinyClothes.Models;

namespace TinyClothes
{
    public static class AccountDb
    {
        public static async Task<bool> IsUsernameTaken(string username, StoreContext context)
        {
            bool isTaken = await (from a in context.Accounts
                                where username == a.Username
                                select a).AnyAsync();
            return isTaken;
        }

        public static async Task<Account> Register(StoreContext context, Account acc)
        {
            await context.Accounts.AddAsync(acc);
            await context.SaveChangesAsync();
            return acc;
        }
    }
}