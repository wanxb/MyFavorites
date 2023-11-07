using Microsoft.Extensions.Options;
using MyFavorites.Core.Models;

namespace MyFavorites.Core.Repository
{
    public class FavoritesRepository : BaseRepository, IFavoritesRepository
    {
        public FavoritesRepository(IOptionsSnapshot<DataBaseSettings> options) : base(options)
        { }

        public async Task<int> DeleteAsync(string id, string uid)
        {
            var items = await GetFavoritesItemsAsync();
            int itemsCount = items.Where(x => x.Fid == id).Count();
            if (itemsCount > 1)
            {
                //删除明细
                string sql = "delete from favorites_items where id=@id;";
                return await base.ExecuteAsync(sql, new { Id = uid });
            }
            else
            {
                //删除类型和明细
                string sql1 = "delete from favorites where id=@id;";
                string sql2 = "delete from favorites_items where id=@id;";
                return await base.ExecuteAsync(sql1, sql2, new { id }, new List<MySQLItems> { new MySQLItems { Id = uid } });
            }
        }

        public async Task<IEnumerable<MySQLFavorites>> GetFavoritesAsync()
        {
            string sql = "select * from favorites where 1=1;";
            return await base.QueryAsync<MySQLFavorites>(sql);
        }

        public async Task<IEnumerable<MySQLItems>> GetFavoritesItemsAsync()
        {
            string sql = "select * from favorites_items where 1=1;";
            return await base.QueryAsync<MySQLItems>(sql);
        }

        public async Task<int> InsertAsync(MySQLFavorites model)
        {
            var sql1 = "insert into favorites (type, sort, description, creationTime, lastModificationTime) values (@type, @sort, @description, @creationTime, @lastModificationTime);";
            var sql2 = "insert into favorites_items (fid, url, name, target, description, sort, creationTime, lastModificationTime) values (@fid, @url, @name, @target, @description, @sort, @creationTime, @lastModificationTime);";
            return await base.ExecuteAsync(sql1, sql2, model, model.Items);
        }

        public async Task<int> UpdateAsync(MySQLFavorites model)
        {
            var sql1 = "update favorites set lastModificationTime=@lastModificationTime where id=@id;";
            var sql2 = "insert into favorites_items (fid, url, name, target, description, sort, creationTime, lastModificationTime) values (@fid, @url, @name, @target, @description, @sort, @creationTime, @lastModificationTime);";
            return await base.ExecuteAsync(sql1, sql2, model, model.Items);
        }
    }
}