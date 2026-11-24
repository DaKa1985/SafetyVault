using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers
{
    [Authorize]
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
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
            var items = await _itemService.GetAllAsync(userId);
            return items.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
            var item = await _itemService.GetByIdAsync(userId, id);
            if (item is null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> AddItem(CreateItemDto dto)
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
            var result = await _itemService.CreateAsync(userId, dto);
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
