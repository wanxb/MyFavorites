using Microsoft.Extensions.Options;
using MyFavorites.Core.Helpers;
using MyFavorites.Core.Models;
using MyFavorites.Core.Models.Dto;
using SharpCompress.Common;
using System.Text;
using System.Text.Json;
using ZstdSharp.Unsafe;

namespace MyFavorites.Core.Services
{
    public class FavoritesFromFileService : IFavoritesService
    {
        private List<FileFavorites> _favorites;
        private string _filePath;

        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="bookStoreDatabaseSettings"></param>
        public FavoritesFromFileService(IOptions<DataBaseSettings> bookStoreDatabaseSettings)
        {
            _filePath = bookStoreDatabaseSettings?.Value?.ConnectionString;

            if (string.IsNullOrEmpty(_filePath))
            {
                _filePath = Path.Combine(Directory.GetCurrentDirectory(), "datasource.json");
            }

            if (!File.Exists(_filePath))
            {
                File.Create(_filePath).Close();
            }

            string jsonString = File.ReadAllText(_filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new StringOrIntConverter());

            _favorites = string.IsNullOrEmpty(jsonString)
                ? new List<FileFavorites>()
                : JsonSerializer.Deserialize<List<FileFavorites>>(jsonString, options);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public Task<List<T>> Get<T>(string keyWord)
        {
            List<T> filteredList = _favorites.Cast<T>().ToList();
            return Task.FromResult(filteredList);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateAsync(FavoritesDto input)
        {
            var favorites = _favorites.Where(p => p.Type == input.Type).FirstOrDefault();
            var items = new FileItems
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
                    Id = Guid.NewGuid().ToString(),
                    Type = input.Type.Trim(),
                    Description = input.Type.Trim(),
                    Sort = sort + 1,
                    Items = new List<FileItems> { items }
                };
                _favorites.Add(favorites);
            }
            else
            {
                int sort = favorites.Items.Any() ? favorites.Items.OrderByDescending(o => o.Sort).First().Sort : 0;
                items.Sort = sort + 1;
                favorites.Items.Add(items);
            }
            await UpdateFileAsync();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string id, string uid)
        {
            FileFavorites favorites = _favorites.FirstOrDefault(p => p.Id == id);
            if (string.IsNullOrEmpty(uid))
                _favorites.Remove(favorites);
            if (favorites.Items.Count > 1)
            {
                var item = favorites.Items.Find(p => p.Id == uid);
                favorites.Items.Remove(item);
            }
            else
            {
                _favorites.Remove(favorites);
            }
            await UpdateFileAsync();
        }

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