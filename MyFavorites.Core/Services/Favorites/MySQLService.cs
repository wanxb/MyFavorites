﻿using MyFavorites.Core.Models;
using MyFavorites.Core.Models.Dto;
using MyFavorites.Core.Repository;

namespace MyFavorites.Core.Services.Favorites
{
    public class MySQLService : FavoritesBaseService, IFavoritesService
    {
        private readonly IFavoritesRepository _favoriteRepository;

        public MySQLService(IFavoritesRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public async Task<List<T>> Get<T>(string keyWord)
        {
            var favorites = await _favoriteRepository.GetFavoritesAsync();
            var favorites_items = await _favoriteRepository.GetFavoritesItemsAsync();

            var query = from favorite in favorites
                        join items in favorites_items on favorite.Id equals items.Fid
                        select new MySQLFavorites
                        {
                            Id = favorite.Id,
                            Type = favorite.Type,
                            Description = favorite.Description,
                            Sort = favorite.Sort,
                            Items = new List<MySQLItems> { items }
                        };
            return query.Cast<T>().ToList();
        }

        /// <summary>
        /// 插入或更新数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateAsync(FavoritesDto input)
        {
            var favoritesAll = await _favoriteRepository.GetFavoritesAsync();
            var favorites = favoritesAll.FirstOrDefault(p => p.Type == input.Type);
            MySQLItems items = new()
            {
                Url = input.Url.Trim(),
                Name = input.Name.Trim(),
                Description = input.Description.Trim(),
                Target = "_blank"
            };

            if (favorites == null)
            {
                int sort = favoritesAll.Any() ? favoritesAll.OrderByDescending(x => x.Sort).First().Sort : 0;
                favorites = new()
                {
                    Type = input.Type.Trim(),
                    Description = input.Type.Trim(),
                    Sort = sort + 1,
                    Items = new List<MySQLItems> { items }
                };
                await _favoriteRepository.InsertAsync(favorites);
            }
            else
            {
                int sort = favorites.Items.Any() ? favorites.Items.OrderByDescending(o => o.Sort).First().Sort : 0;
                items.Sort = sort + 1;
                favorites.Items.Add(items);
                await _favoriteRepository.UpdateAsync(favorites);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string id, string uid)
        {
            await _favoriteRepository.DeleteAsync(id, uid);
        }
    }
}