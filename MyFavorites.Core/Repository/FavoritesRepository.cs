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
            if (itemsCount == 0)
            {
                //删除类型和明细
                string sql1 = "delete from favorites where id=@id;";
                string sql2 = "delete from favorites_items where id=@uid;";
                return await base.ExecuteAsync(sql1, sql2, id, uid);
            }
            else
            {
                //删除类型
                string sql = "delete from favorites_items where id=@uid;";
                return await base.ExecuteAsync(sql, uid);
            }
        }

        public async Task<IEnumerable<MySQLFavorites>> GetFavoritesAsync()
        {
            string sql = "select * favorites where 1=1;";
            return await base.QueryAsync<MySQLFavorites>(sql);
        }

        public async Task<MySQLFavorites> GetFavoritesAsync(long id)
        {
            string sql = "select * favorites where id = @id;";
            return await base.QueryFirstOrDefaultAsync<MySQLFavorites>(sql, id);
        }

        public async Task<MySQLItems> GetFavoritesItemsAsync(long id)
        {
            string sql = "select * favorites_items where id = @id;";
            return await base.QueryFirstOrDefaultAsync<MySQLItems>(sql, id);
        }

        public async Task<IEnumerable<MySQLItems>> GetFavoritesItemsAsync()
        {
            string sql = "select * from favorites_items where 1=1;";
            return await base.QueryAsync<MySQLItems>(sql);
        }

        public async Task<int> InsertAsync(MySQLFavorites model)
        {
            var sql1 = "insert into favorites (type, sort, description, creationTime, lastModificationTime) values (@type, @sort, @description, @creationTime, @lastModificationTime);";
            var sql2 = "insert into favorites_items (id, fid, url, name, target, description, sort, creationTime, lastModificationTime) values (@id, @fid, @url, @name, @target, @description, @sort, @creationTime, @lastModificationTime);";
            return await base.ExecuteAsync(sql1, sql2, model, model.Items);
        }

        public async Task<int> UpdateAsync(MySQLFavorites model)
        {
            var sql1 = "update favorites set lastModificationTime=@lastModificationTime where id=@id;";
            var sql2 = "insert into favorites_items (id, fid, url, name, target, description, sort, creationTime, lastModificationTime) values (@id, @fid, @url, @name, @target, @description, @sort, @creationTime, @lastModificationTime);";
            return await base.ExecuteAsync(sql1, sql2, model, model.Items);
        }
    }
}