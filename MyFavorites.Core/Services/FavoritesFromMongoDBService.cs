﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyFavorites.Core.Models;
using MyFavorites.Core.Models.Dto;

namespace MyFavorites.Core.Services
{
    public class FavoritesFromMongoDBService : FavoritesBaseService, IFavoritesService
    {
        private readonly IMongoCollection<MongoDBFavorites> _favoritesCollection;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bookStoreDatabaseSettings"></param>
        public FavoritesFromMongoDBService(IOptions<DataBaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _favoritesCollection = mongoDatabase.GetCollection<MongoDBFavorites>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        /// <summary>
        /// 插入或更新数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateAsync(FavoritesDto input)
        {
            var favorites = await _favoritesCollection.Find(x => x.Type == input.Type).FirstOrDefaultAsync();

            MongoDBItems items = new()
            {
                Id = Guid.NewGuid().ToString(),
                Url = input.Url.Trim(),
                Name = input.Name.Trim(),
                Description = input.Description.Trim(),
                Target = "_blank"
            };

            if (favorites == null)
            {
                int sort = _favoritesCollection.AsQueryable().Any() ? _favoritesCollection.AsQueryable().OrderByDescending(o => o.Sort).First().Sort : 0;
                favorites = new()
                {
                    Type = input.Type.Trim(),
                    Description = input.Type.Trim(),
                    Sort = sort + 1,
                    Items = new List<MongoDBItems> { items }
                };
                await CreateAsync(favorites);
            }
            else
            {
                int sort = favorites.Items.Any() ? favorites.Items.OrderByDescending(o => o.Sort).First().Sort : 0;
                items.Sort = sort + 1;
                favorites.Items.Add(items);
                await UpdateAsync(favorites.Id, favorites);
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
            if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(uid))
                await RemoveAsync(id);
            var favorites = await _favoritesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (favorites.Items.Count > 1)
            {
                var item = favorites.Items.Find(p => p.Id == uid);
                favorites.Items.Remove(item);
                await UpdateAsync(favorites.Id, favorites);
            }
            else
            {
                await RemoveAsync(id);
            }
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <returns></returns>
        //public async Task<List<Favorites>> Get(string keyWord)
        //{
        //    return await _favoritesCollection.Find(_ => true).ToListAsync();
        //}
        public async Task<List<T>> Get<T>(string keyWord)
        {
            var data = await _favoritesCollection.Find(_ => true).ToListAsync();
            return data.Cast<T>().ToList();
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="newFavorites"></param>
        /// <returns></returns>
        private async Task CreateAsync(MongoDBFavorites newFavorites) =>
            await _favoritesCollection.InsertOneAsync(newFavorites);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedFavorites"></param>
        /// <returns></returns>
        private async Task UpdateAsync(string id, MongoDBFavorites updatedFavorites) =>
            await _favoritesCollection.ReplaceOneAsync(x => x.Id == id, updatedFavorites);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private async Task RemoveAsync(string id) =>
            await _favoritesCollection.DeleteOneAsync(x => x.Id == id);
    }
}