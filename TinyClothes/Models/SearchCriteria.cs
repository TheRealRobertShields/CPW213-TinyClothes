using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TinyClothes.Models
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            Results = new List<Clothing>();
        }

        public string Size { get; set; }

        /// <summary>
        /// Type of clothing (shirt, pants, hat, etc.)
        /// </summary>
        public string Type { get; set; }
        
        [StringLength(150)]
        public string Title { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Minimum price must be positive")]
        [Display(Name = "Minimum Price")]
        public double? MinPrice { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Maximum price must be positive")]
        [Display(Name = "Max Price")]
        public double? MaxPrice { get; set; }

        public List<Clothing> Results { get; set; }

        /// <summary>
        /// Returns true if at least one search criteria is provided.
        /// </summary>
        /// <returns></returns>
        public bool IsSearching()
        {
            if (MaxPrice.HasValue || MinPrice.HasValue || string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Size) || string.IsNullOrWhiteSpace(Type))
            {
                return true;
            }
            return false;
        }

    }
}
