using MyFavorites.Core.Models;
using MyFavorites.Core.Models.Dto;

namespace MyFavorites.Core.Services
{
    public interface IFavoritesService
    {
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        Task<List<Favorites>> Get();

        /// <summary>
        ///根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Favorites?> GetAsync(string id);

        /// <summary>
        /// 插入或更新数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdateAsync(FavoritesDto input);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>

        Task RemoveAsync(string id, string uid);
    }
}