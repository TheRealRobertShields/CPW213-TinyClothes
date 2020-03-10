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
        public static async Task<List<Clothing>> GetClothingByPage(StoreContext context, int pageNum, int pageSize)
        {
            // To get page 1, we wouldn't skip any rows, so we must offset by 1.
            const int PageOffset = 1;

            // LINQ Method Syntax
            List<Clothing> clothes = await context.Clothing
                .OrderBy(c => c.ItemId)     // OrderBy can go before Skip
                .Skip(pageSize * (pageNum - PageOffset))    // Must Skip before Take
                .Take(pageSize)
                .ToListAsync();

            return clothes;

            // LINQ Query Syntax
            //List<Clothing> clothes2 = await (from c in context.Clothing
            //                                 orderby c.ItemId ascending
            //                                 select c)
            //                                 .Skip(pageSize * (pageNum - PageOffset))    
            //                                 .Take(pageSize)
            //                                 .ToListAsync(); 
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
        /// Returns a single clothing item or null if there is no match.
        /// </summary>
        /// <param name="id">id of the clothing item</param>
        /// <param name="context">DB context</param>
        public static async Task<Clothing> GetClothingById(int id, StoreContext context)
        {
            Clothing c = await (from clothing in context.Clothing
                                where clothing.ItemId == id
                                select clothing).SingleOrDefaultAsync();
            return c;
        }

        public static async Task<Clothing> Edit(Clothing c, StoreContext context)
        {
            await context.AddAsync(c);  // add clothing to context
            context.Entry(c).State = EntityState.Modified;  // mark it as modified
            await context.SaveChangesAsync();   // update DB
            return c;
        }

        /// <summary>
        /// Returns the total number of clothing items.
        /// </summary>
        public static async Task<int> GetNumClothing(StoreContext context)
        {
            return await context.Clothing.CountAsync();

            // Alternative query syntax
            //return await (from c in context.Clothing select c).CountAsync()
        }


        public static async Task Delete(Clothing c, StoreContext context)
        {
            await context.AddAsync(c);
            context.Entry(c).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public static async Task<SearchCriteria> BuildSearchQueryAsync(SearchCriteria search, StoreContext context)
        {
            // Prepare query - SELECT * FROM Clothes
            // Does not get sent to DB
            IQueryable<Clothing> allClothes = (from c in context.Clothing
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

            search.Results = await allClothes.ToListAsync();
            return search;
        }
    }
}
