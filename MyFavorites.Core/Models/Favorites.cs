using System.ComponentModel.DataAnnotations;

namespace MyFavorites.Core.Models
{
    public class Favorites
    {
        [Required]
        public string Type { get; set; }

        public int Sort { get; set; }

        public string? Description { get; set; }
    }
}