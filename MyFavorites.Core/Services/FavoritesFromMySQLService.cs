﻿using MyFavorites.Core.Models;
using MyFavorites.Core.Models.Dto;
using MyFavorites.Core.Repository;

namespace MyFavorites.Core.Services
{
    public class FavoritesFromMySQLService : IFavoritesService
    {
        private readonly IFavoritesRepository _favoriteRepository;

        public FavoritesFromMySQLService(IFavoritesRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task CreateOrUpdateAsync(FavoritesDto input)
        {
            var favoritesAll = await _favoriteRepository.GetFavoritesAsync();
            var favorites = favoritesAll.FirstOrDefault(p => p.Type == input.Type);
            Items items = new()
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
                    Items = new List<Items> { items }
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

        public async Task<List<Favorites>> Get()
        {
            var favorites = await _favoriteRepository.GetFavoritesAsync();
            var favorites_items = await _favoriteRepository.GetFavoritesItemsAsync();

            var query = from favorite in favorites
                        join items in favorites_items on favorite.Id equals items.Fid
                        select new Favorites
                        {
                            Id = favorite.Id,
                            Type = favorite.Type,
                            Description = favorite.Description,
                            Sort = favorite.Sort,
                            Items = new List<Items> { items }
                        };

            return query.ToList();
        }

        public Task<Favorites?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(string id, string uid)
        {
            await _favoriteRepository.DeleteAsync(id, uid);
        }
    }
}