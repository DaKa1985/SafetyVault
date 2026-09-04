using Microsoft.AspNetCore.Mvc;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChecklistsController : ControllerBase
{
    private static readonly List<ChecklistDto> Checklists = new List<ChecklistDto>
    {
        new ChecklistDto { Id = 1, Name = "Notfallrucksack", Description = "Alles für den Rucksack", Items = new List<ChecklistItemDto>() },
        new ChecklistDto { Id = 2, Name = "Verpflegungskoffer", Description = "Verpflegungskoffer für 7 Tage", Items = new List<ChecklistItemDto>() },
        new ChecklistDto { Id = 3, Name = "technisches Material", Description = "Technisches Material für den Notfall", Items = new List<ChecklistItemDto>() }
    };

    private static readonly List<ChecklistItemDto> ChecklistItems = new List<ChecklistItemDto>
    {
        new ChecklistItemDto { Id = 1, Name = "Wasser", Description = "Wasser für 7 Tage",Quantity = 14, IsPacked = false, ChecklistId = 2 },
        new ChecklistItemDto { Id = 2, Name = "Essen", Description = "Essen für 7 Tage", Quantity = 7,IsPacked = false, ChecklistId = 2 },
        new ChecklistItemDto { Id = 3, Name = "Medikamente", Description = "Medikamente für 7 Tage",Quantity = 4, IsPacked = false, ChecklistId = 2 },
        new ChecklistItemDto { Id = 4, Name = "Taschenlampe", Description = "Taschenlampe mit Batterien",Quantity = 2, IsPacked = false, ChecklistId = 1 },
        new ChecklistItemDto { Id = 5, Name = "Batterien", Description = "Batterien für Taschenlampe",Quantity = 4, IsPacked = false, ChecklistId = 1 },
        new ChecklistItemDto { Id = 6, Name = "Erste-Hilfe-Set", Description = "Erste-Hilfe-Set für Notfälle",Quantity = 1, IsPacked = false, ChecklistId = 1 }
    };

    [HttpGet]
    public ActionResult<IEnumerable<ChecklistSummaryDto>> GetAll()
    {
        return Ok(Checklists.Select(c => new ChecklistSummaryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            ItemCount = ChecklistItems.Count(item => item.ChecklistId == c.Id)
        }));
    }

    [HttpGet("{id}")]
    public ActionResult<ChecklistDto> GetById(int id)
    {
        var checklist = Checklists.FirstOrDefault(c => c.Id == id);
        if (checklist == null)
        {
            return NotFound();
        }

        var result = new ChecklistDto
        {
            Id = checklist.Id,
            Name = checklist.Name,
            Description = checklist.Description,
            Items = ChecklistItems.Where(item => item.ChecklistId == checklist.Id).ToList()
        };
        return Ok(result);
    }

    [HttpGet("/api/checklists/{checklistId}/items/{id}")]
    public ActionResult<ChecklistItemDto> GetItemById(int checklistId, int id)
    {
        var item = ChecklistItems.FirstOrDefault(i => i.ChecklistId == checklistId && i.Id == id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost("/api/checklists/{checklistId}/items")]
    public ActionResult<ChecklistItemDto> AddItem(int checklistId, CreateChecklistItemDto dto)
    {
        var checklist = Checklists.FirstOrDefault(c => c.Id == checklistId);
        if (checklist == null)
        {
            return NotFound();
        }
        var newItem = new ChecklistItemDto
        {
            Id = ChecklistItems.Count == 0 ? 1 : ChecklistItems.Max(i => i.Id) + 1,
            Name = dto.Name,
            Description = dto.Description,
            Quantity = dto.Quantity,
            IsPacked = false,
            PackedAt = null,
            ChecklistId = checklistId
        };
        ChecklistItems.Add(newItem);
        return CreatedAtAction(nameof(GetItemById), new { checklistId = checklistId, id = newItem.Id }, newItem);
    }
}
