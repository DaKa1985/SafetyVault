using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers;

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
    public ActionResult<ChecklistDto> GetById(int id)
    {
        var checklist = _checklistService.GetById(id);
        if (checklist == null)
        {
            return NotFound();
        }
        return Ok(checklist);
    }

    [HttpGet("{checklistId}/items/{id}")]
    public ActionResult<ChecklistItemDto> GetItemById(int checklistId, int id)
    {
        var item = _checklistService.GetItem(checklistId, id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost("{checklistId}/items")]
    public ActionResult<ChecklistItemDto> AddItem(int checklistId, CreateChecklistItemDto dto)
    {
        var newItem = _checklistService.AddItem(checklistId, dto);
        if (newItem == null)
            return NotFound();
        return CreatedAtAction(nameof(GetItemById), new { checklistId, id = newItem.Id }, newItem);
    }

    [HttpPut("{checklistId}/items/{id}")]
    public ActionResult UpdateItem(int checklistId, int id, UpdateChecklistItemDto dto)
    {
        var success = _checklistService.UpdateItem(checklistId, id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{checklistId}/items/{id}")]

    public ActionResult DeleteItem(int checklistId, int id)
    {
        var success = _checklistService.DeleteItem(checklistId, id);
        return success ? NoContent() : NotFound();
    }
}

