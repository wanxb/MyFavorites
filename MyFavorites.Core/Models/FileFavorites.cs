namespace MyFavorites.Core.Models
{
    public class FileFavorites : Favorites
    {
        public string Id { get; set; }
        public List<FileItems>? Items { get; set; }
    }

    public class FileItems : FavoritesItems
    {
        public string Id { get; set; }
    }
}