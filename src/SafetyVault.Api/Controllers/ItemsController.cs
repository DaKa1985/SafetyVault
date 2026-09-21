using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Entities;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {

        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService
                ?? throw new ArgumentNullException(nameof(itemService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll()
        {
            var items = await _itemService.GetAllAsync();
            return items.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> AddItem(CreateItemDto dto)
        {
            var result = await _itemService.CreateAsync(dto);
            switch (result.Status)
            {
                case CreateItemStatus.Success:
                    return CreatedAtAction(nameof(GetById), new {id = result.Item!.Id }, result.Item);
                case CreateItemStatus.NameAlreadyExists:
                    return Conflict($"Item with the name '{dto.Name}' already exists.");
                default:
                    throw new InvalidOperationException($"Unexpected status: {result.Status}");
            }
        }
    }
}
