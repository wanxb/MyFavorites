using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyFavorites.Core.Models
{
    public class MongoDBFavorites : Favorites
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public List<MongoDBItems>? Items { get; set; }
    }

    public class MongoDBItems : FavoritesItems
    {
        public string Uid { get; set; }

        public string Id
        {
            get { return Uid; }
            set { Uid = value; }
        }
    }
}