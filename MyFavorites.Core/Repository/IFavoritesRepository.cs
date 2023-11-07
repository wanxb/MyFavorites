using MyFavorites.Core.Models;

namespace MyFavorites.Core.Repository
{
    public interface IFavoritesRepository
    {
        /// <summary>
        /// 查询全部数据
        /// </summary>
        Task<IEnumerable<MySQLFavorites>> GetFavoritesAsync();

        /// <summary>
        /// 查询明细全部数据
        /// </summary>
        Task<IEnumerable<MySQLItems>> GetFavoritesItemsAsync();

        /// <summary>
        /// 插入
        /// </summary>
        Task<int> InsertAsync(MySQLFavorites model);

        /// <summary>
        /// 更新
        /// </summary>
        Task<int> UpdateAsync(MySQLFavorites model);

        /// <summary>
        /// 删除
        /// </summary>
        Task<int> DeleteAsync(string id, string uid);
    }
}