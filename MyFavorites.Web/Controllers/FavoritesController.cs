using Microsoft.AspNetCore.Mvc;
using MyFavorites.Core.Models;
using MyFavorites.Core.Models.Dto;
using MyFavorites.Core.Services;

namespace MyFavorites.Web.Controllers
{
    public class FavoritesController : FavoritesBaseController
    {
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(IFavoritesService booksService) =>
            _favoritesService = booksService;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<dynamic> Get(string keyWord) =>
            await _favoritesService.Get<dynamic>(keyWord);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FavoritesDto input)
        {
            await _favoritesService.CreateOrUpdateAsync(input);
            return CreatedAtAction(nameof(Get), new { id = input.Id }, input);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] FavoritesDto input)
        {
            await _favoritesService.RemoveAsync(input.Id, input.Uid);
            return NoContent();
        }
    }
}