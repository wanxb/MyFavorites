using System.ComponentModel.DataAnnotations;

namespace MyFavorites.Core.Models
{
    public class FavoritesItems
    {
        [Required]
        public string Url { get; set; }

        public string? Target { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Sort { get; set; }
    }
}