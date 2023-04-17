using Microsoft.AspNetCore.Mvc;
using RankingApp.Models;

namespace RankingApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private static readonly IEnumerable<ItemModel> Items = new[]
        {
            new ItemModel {Id = 1, Title = "Resident Evil", ImageId = 1, Ranking = 0, ItemType = 1},
            new ItemModel {Id = 2, Title = "Resident Evil 2", ImageId = 2, Ranking = 0, ItemType = 1},
            new ItemModel {Id = 3, Title = "Resident Evil 3", ImageId = 3, Ranking = 0, ItemType = 1},
            new ItemModel {Id = 4, Title = "Resident Evil 4", ImageId = 4, Ranking = 0, ItemType = 1},
            new ItemModel {Id = 5, Title = "Resident Evil 5", ImageId = 5, Ranking = 0, ItemType = 1},
            new ItemModel {Id = 6, Title = "Resident Evil 6", ImageId = 6, Ranking = 0, ItemType = 1},
            new ItemModel {Id = 7, Title = "Resident Evil 7", ImageId = 7, Ranking = 0, ItemType = 1},
            new ItemModel {Id = 8, Title = "Resident Evil 8", ImageId = 8, Ranking = 0, ItemType = 1},

            new ItemModel {Id = 9, Title = "C#", ImageId = 9, Ranking = 0, ItemType = 2},
            new ItemModel {Id = 10, Title = "Java", ImageId = 10, Ranking = 0, ItemType = 2},
            new ItemModel {Id = 11, Title = "C++", ImageId = 11, Ranking = 0, ItemType = 2},
            new ItemModel {Id = 12, Title = "Python", ImageId = 12, Ranking = 0, ItemType = 2},
            new ItemModel {Id = 13, Title = "JavaScript", ImageId = 13, Ranking = 0, ItemType = 2},
            new ItemModel {Id = 14, Title = "TypeScript", ImageId = 14, Ranking = 0, ItemType = 2},
            new ItemModel {Id = 15, Title = "PHP", ImageId = 15, Ranking = 0, ItemType = 2},
            new ItemModel {Id = 16, Title = "SQL", ImageId = 16, Ranking = 0, ItemType = 2}
        };

        [HttpGet("{itemType:int}")]
        public ItemModel[] Get(int ItemType)
        {
            ItemModel[] items = Items.Where(i => i.ItemType == ItemType).ToArray();
            return items;
        }
    }
}
