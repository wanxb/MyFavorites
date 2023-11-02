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
        public async Task<List<Favorites>> Get() =>
            await _favoritesService.Get();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Favorites>> Get(string id)
        {
            var favorites = await _favoritesService.GetAsync(id);

            if (favorites is null)
            {
                return NotFound();
            }

            return favorites;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FavoritesDto input)
        {
            await _favoritesService.CreateOrUpdateAsync(input);

            return CreatedAtAction(nameof(Get), new { id = input.Id }, input);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] FavoritesDto input)
        {
            var favorites = await _favoritesService.GetAsync(input.Id);

            if (favorites is null)
            {
                return NotFound();
            }
            if (!favorites.Items.Any(p => p.Id == input.Uid))
            {
                return NotFound();
            }

            await _favoritesService.RemoveAsync(input.Id, input.Uid);

            return NoContent();
        }
    }
}