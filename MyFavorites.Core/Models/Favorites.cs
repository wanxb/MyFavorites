using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyFavorites.Core.Models
{
    public class Favorites
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Type { get; set; } = null!;

        public int Sort { get; set; }

        public string? Description { get; set; }

        public List<Items>? Items { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }

    public class Items
    {
        public string? Fid { get; set; }

        public string? Id { get; set; }

        public string Url { get; set; } = null!;

        public string? Target { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int Sort { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}