using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TinyClothes.Models
{
    /// <summary>
    /// Represents a single clothing item
    /// </summary>
    public class Clothing
    {
        /// <summary>
        /// The unique identifier for the clothing item
        /// </summary>
        [Key] //Set as PK
        public int ItemId { get; set; }

        public string Size { get; set; }

        /// <summary>
        /// Type of clothing (shirt, pants, etc.)
        /// </summary>
        public string Type { get; set; }

        public string Color { get; set; }

        /// <summary>
        /// Retail price of the item
        /// </summary>
        [Range(0.0, 10000.0)]
        public double Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
