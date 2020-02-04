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

        [Required(ErrorMessage = "Size is required")]
        public string Size { get; set; }

        /// <summary>
        /// Type of clothing (shirt, pants, etc.)
        /// </summary>
        [Required]
        public string Type { get; set; }

        [Required]
        public string Color { get; set; }

        /// <summary>
        /// Retail price of the item
        /// </summary>
        [Required] //<-- don't need to specify required. doubles are required by default
        [Range(0.0, 100000.0)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Required. No spaces or special characters")]
        [StringLength(35)]
        [RegularExpression("^([A-Za-z0-9])+$")]
        public string Title { get; set; }

        [Required]
        [StringLength(800)]
        public string Description { get; set; }
    }
}
