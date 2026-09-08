using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public class ChecklistService : IChecklistService
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
            new ChecklistItemDto { Id = 4, Name = "Taschenlampe LED", Description = "Taschenlampe mit Batterien",Quantity = 2, IsPacked = false, ChecklistId = 1 },
            new ChecklistItemDto { Id = 5, Name = "Batterien", Description = "Batterien für Taschenlampe",Quantity = 4, IsPacked = false, ChecklistId = 1 },
            new ChecklistItemDto { Id = 6, Name = "Erste-Hilfe-Set", Description = "Erste-Hilfe-Set für Notfälle",Quantity = 1, IsPacked = false, ChecklistId = 1 }
        };
        public ChecklistItemDto? AddItem(int checklistId, CreateChecklistItemDto dto)
        {
            var checklist = Checklists.FirstOrDefault(c => c.Id == checklistId);
            if (checklist == null)
            {
                return null;
            }
            var newItem = new ChecklistItemDto
            {
                Id = ChecklistItems.Count == 0 ? 1 : ChecklistItems.Max(i => i.Id) + 1,
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.Quantity,
                IsPacked = false,
                ChecklistId = checklistId
            };
            ChecklistItems.Add(newItem);
            return newItem;
        }
        public IEnumerable<ChecklistSummaryDto> GetAll()
        {
            var checklistSummaries = Checklists.Select(c => new ChecklistSummaryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ItemCount = ChecklistItems.Count(i => i.ChecklistId == c.Id)
            }).ToList();
            return checklistSummaries;
        }
        public ChecklistDto? GetById(int id)
        {
            var checklist = Checklists.FirstOrDefault(c => c.Id == id);
            if (checklist == null)
            {
                return null;
            }
            return new ChecklistDto
            {
                Id = checklist.Id,
                Name = checklist.Name,
                Description = checklist.Description,
                Items = ChecklistItems.Where(i => i.ChecklistId == id).ToList()
            };
        }
        public ChecklistItemDto? GetItem(int checklistId, int itemId)
        {
            var item = ChecklistItems.FirstOrDefault(i => i.ChecklistId == checklistId && i.Id == itemId);
            return item;
        }
        public bool UpdateItem(int checklistId, int itemId, UpdateChecklistItemDto dto)
        {
            var item = ChecklistItems.FirstOrDefault(i => i.ChecklistId == checklistId && i.Id == itemId);
            if (item == null)
            {
                return false;
            }
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Quantity = dto.Quantity;

            bool wasPackedBefore = item.IsPacked;
            item.IsPacked = dto.IsPacked;

            if (!wasPackedBefore && dto.IsPacked)
            {
                item.PackedAt = DateTime.UtcNow;
            }
            else if (!dto.IsPacked)
            {
                item.PackedAt = null;
            }
            return true;
        }

        public bool DeleteItem(int checklistId, int itemId)
        {
            var item = ChecklistItems.FirstOrDefault(i => i.ChecklistId == checklistId && i.Id == itemId);
            if (item == null)
            {
                return false;
            }
            ChecklistItems.Remove(item);
            return true;
        }
    }
}
