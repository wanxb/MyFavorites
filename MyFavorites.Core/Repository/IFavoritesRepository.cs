using MyFavorites.Core.Models;

namespace MyFavorites.Core.Repository
{
    public interface IFavoritesRepository
    {
        /// <summary>
        /// 查询全部数据
        /// </summary>
        Task<IEnumerable<Favorites>> GetFavoritesAsync();

        /// <summary>
        /// 查询明细全部数据
        /// </summary>
        Task<IEnumerable<Items>> GetFavoritesItemsAsync();

        /// <summary>
        /// 查询实体
        /// </summary>
        Task<Favorites> GetFavoritesAsync(string id);

        /// <summary>
        /// 查询明细实体
        /// </summary>
        Task<Items> GetFavoritesItemsAsync(string id);

        /// <summary>
        /// 插入
        /// </summary>
        Task<int> InsertAsync(Favorites model);

        /// <summary>
        /// 更新
        /// </summary>
        Task<int> UpdateAsync(Favorites model);

        /// <summary>
        /// 删除
        /// </summary>
        Task<int> DeleteAsync(string id, string uid);
    }
}