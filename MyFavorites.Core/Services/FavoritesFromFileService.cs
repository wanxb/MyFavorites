using Microsoft.Extensions.Options;
using MyFavorites.Core.Models.Dto;
using MyFavorites.Core.Helpers;
using System.Text;
using System.Text.Json;
using MyFavorites.Core.Models;

namespace MyFavorites.Core.Services
{
    public class FavoritesFromFileService : IFavoritesService
    {
        private readonly IEnumerable<Favorites> _favorites;
        private readonly string _filePath;

        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="bookStoreDatabaseSettings"></param>
        public FavoritesFromFileService(IOptions<DataBaseSettings> bookStoreDatabaseSettings)
        {
            _filePath = bookStoreDatabaseSettings.Value.ConnectionString;
            string jsonString = System.IO.File.ReadAllText(_filePath);
            if (string.IsNullOrEmpty(jsonString)) return;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new StringOrIntConverter());
            _favorites = JsonSerializer.Deserialize<List<Favorites>>(jsonString, options);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateAsync(FavoritesDto input)
        {
            var favorites = _favorites.Where(p => p.Type == input.Type).FirstOrDefault();
            var items = new Items
            {
                Id = Guid.NewGuid().ToString(),
                Url = input.Url.Trim(),
                Name = input.Name.Trim(),
                Description = input.Description.Trim(),
                Target = "_blank"
            };
            if (favorites == null)
            {
                int sort = _favorites.Any() ? _favorites.OrderByDescending(o => o.Sort).First().Sort : 0;
                favorites = new()
                {
                    Type = input.Type.Trim(),
                    Description = input.Type.Trim(),
                    Sort = sort + 1,
                    Items = new List<Items> { items }
                };
            }
            else
            {
                int sort = favorites.Items.Any() ? favorites.Items.OrderByDescending(o => o.Sort).First().Sort : 0;
                items.Sort = sort + 1;
                favorites.Items.Add(items);
            }
            _favorites.Append(favorites);
            await UpdateFileAsync();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string id, string uid)
        {
            Favorites favorites = await GetAsync(id);
            if (string.IsNullOrEmpty(uid))
                _favorites.ToList().Remove(favorites);
            var item = favorites.Items.Find(p => p.Id == uid);
            favorites.Items.Remove(item);
            await UpdateFileAsync();
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public Task<List<Favorites>> Get() => Task.FromResult(_favorites.ToList());

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Favorites> GetAsync(string id) => Task.FromResult(_favorites.FirstOrDefault(p => p.Id == id) ?? new Favorites());

        /// <summary>
        /// 更新文件数据
        /// </summary>
        /// <returns></returns>
        private Task UpdateFileAsync()
        {
            var jsonString = JsonSerializer.Serialize(_favorites);
            using StreamWriter sw = new(_filePath, false, Encoding.UTF8);
            sw.Write(jsonString);
            return Task.CompletedTask;
        }
    }
}