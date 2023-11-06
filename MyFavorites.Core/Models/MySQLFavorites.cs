namespace MyFavorites.Core.Models
{
    public class MySQLFavorites : Favorites
    {
        public string Id { get; set; }
        public List<MySQLItems>? Items { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }

    public class MySQLItems : FavoritesItems
    {
        public string Fid { get; set; }

        public string Id { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}