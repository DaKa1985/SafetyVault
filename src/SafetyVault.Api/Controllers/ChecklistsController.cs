using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChecklistsController : ControllerBase
    {
        private readonly IChecklistService _checklistService;

        public ChecklistsController(IChecklistService checklistService)
        {
            _checklistService = checklistService
                ?? throw new ArgumentNullException(nameof(checklistService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChecklistSummaryDto>>> GetAll()
        {
            int userId = int.Parse(User.FindFirst("sub")!.Value);
            var checklists = await _checklistService.GetAllAsync(userId);
            return Ok(checklists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChecklistDto>> GetById(int id)
        {
            int userId = int.Parse(User.FindFirst("sub")!.Value);
            var checklist = await _checklistService.GetByIdAsync(userId, id);
            if (checklist is null)
            {
                return NotFound();
            }
            return Ok(checklist);
        }

        [HttpGet("{checklistId}/items/{id}")]
        public async Task<ActionResult<ChecklistItemDto>> GetItemById(int checklistId, int id)
        {
            int userId = int.Parse(User.FindFirst("sub")!.Value);
            var item = await _checklistService.GetItemAsync(userId, checklistId, id);
            if (item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost("{checklistId}/items")]
        public async Task<ActionResult<ChecklistItemDto>> AddItem(int checklistId, CreateChecklistItemDto dto)
        {
            int userId = int.Parse(User.FindFirst("sub")!.Value);
            var result = await _checklistService.AddItemAsync(userId, checklistId, dto);
            switch (result.Status)
            {
                case AddChecklistItemStatus.Success:
                    return CreatedAtAction(nameof(GetItemById), new { checklistId, id = result.Item!.Id }, result.Item);
                case AddChecklistItemStatus.ChecklistNotFound:
                    return NotFound("Checklist not found");
                case AddChecklistItemStatus.ItemNotFound:
                    return BadRequest("Item not found");
                default:
                    throw new InvalidOperationException($"Unexpected status: {result.Status}");
            }
        }

        [HttpPut("{checklistId}/items/{id}")]
        public async Task<ActionResult<ChecklistItemDto>> UpdateItem(int checklistId, int id, UpdateChecklistItemDto dto)
        {
            int userId = int.Parse(User.FindFirst("sub")!.Value);
            var updated = await _checklistService.UpdateItemAsync(userId, checklistId, id, dto);
            return updated is not null ? Ok(updated) : NotFound();
        }

        [HttpDelete("{checklistId}/items/{id}")]
        public async Task<ActionResult> DeleteItem(int checklistId, int id)
        {
            int userId = int.Parse(User.FindFirst("sub")!.Value);
            var success = await _checklistService.DeleteItemAsync(userId, checklistId, id);
            return success ? NoContent() : NotFound();
        }
    }
}