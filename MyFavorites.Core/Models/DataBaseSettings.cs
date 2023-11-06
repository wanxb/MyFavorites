namespace MyFavorites.Core.Models
{
    public class DataBaseSettings
    {
        public const string SectionName = "MyFavorites";

        public string ConnectionString { get; set; } = null!;

        public DatabaseType DatabaseType { get; set; }

        public string DatabaseName { get; set; } = null!;

        public string BooksCollectionName { get; set; } = null!;
    }
    public enum DatabaseType
    {
        Default,
        MySQL,
        MongoDB,
        File
    }
}