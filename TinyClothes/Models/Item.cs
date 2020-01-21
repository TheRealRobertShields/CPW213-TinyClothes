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
    public class Item
    {
        /// <summary>
        /// The unique identifier for the clothing item
        /// </summary>
        [Key] //Set as PK
        public int ItemId { get; set; }

    }
}
