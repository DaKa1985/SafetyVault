using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers
{

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
            var checklists = await _checklistService.GetAllAsync();
            return Ok(checklists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChecklistDto>> GetById(int id)
        {
            var checklist = await _checklistService.GetByIdAsync(id);
            if (checklist is null)
            {
                return NotFound();
            }
            return Ok(checklist);
        }

        [HttpGet("{checklistId}/items/{id}")]
        public async Task<ActionResult<ChecklistItemDto>> GetItemById(int checklistId, int id)
        {
            var item = await _checklistService.GetItemAsync(checklistId, id);
            if (item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost("{checklistId}/items")]
        public async Task<ActionResult<ChecklistItemDto>> AddItem(int checklistId, CreateChecklistItemDto dto)
        {
            var result = await _checklistService.AddItemAsync(checklistId, dto);
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
        public async Task<ActionResult> UpdateItem(int checklistId, int id, UpdateChecklistItemDto dto)
        {
            var success = await _checklistService.UpdateItemAsync(checklistId, id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{checklistId}/items/{id}")]
        public async Task<ActionResult> DeleteItem(int checklistId, int id)
        {
            var success = await _checklistService.DeleteItemAsync(checklistId, id);
            return success ? NoContent() : NotFound();
        }
    }
}