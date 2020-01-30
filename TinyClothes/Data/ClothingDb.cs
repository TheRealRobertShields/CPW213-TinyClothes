using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyClothes.Models;

namespace TinyClothes.Data
{
    /// <summary>
    /// Contains DB helper methods for <see cref="Models.Clothing"/>
    /// </summary>
    public static class ClothingDb
    {
        /// <summary>
        /// Returns a specific page of clothing items sorted by ItemId in ascending order
        /// </summary>
        /// <param name="context">The DB context </param>
        /// <param name="pageNum">The page number</param>
        /// <param name="pageSize">The number of clothing items per page</param>
        public async static Task<List<Clothing>> GetClothingByPage(StoreContext context, int pageNum, int pageSize)
        {
            // To get page 1, we wouldn't skip any rows, so we must offset by 1.
            const int PageOffset = 1;

            // LINQ Method Syntax
            List<Clothing> clothes = await context.Clothing
                .Skip(pageSize * (pageNum - PageOffset))    // Must Skip before Take
                .Take(pageSize)
                .OrderBy(c => c.ItemId)     // OrderBy can go before Skip
                .ToListAsync();

            return clothes;

            // LINQ Query Syntax
            List<Clothing> clothes2 = await (from c in context.Clothing
                                             orderby c.ItemId ascending
                                             select c)
                                             .Skip(pageSize * (pageNum - PageOffset))    
                                             .Take(pageSize)
                                             .ToListAsync(); 
        }

        /// <summary>
        /// Adds a clothing object to the DB.
        /// Returns the object with the ID populated.
        /// </summary>
        public static async Task<Clothing> Add(Clothing c, StoreContext context)
        {
            await context.AddAsync(c);  // prepares INSERT query
            await context.SaveChangesAsync();  // execute INESRT query

            return c;
        }

        /// <summary>
        /// Returns the total number of clothing items.
        /// </summary>
        public async static Task<int> GetNumClothing(StoreContext context)
        {
            return await context.Clothing.CountAsync();

            // Alternative query syntax
            //return await (from c in context.Clothing select c).CountAsync()
        }
    }
}
