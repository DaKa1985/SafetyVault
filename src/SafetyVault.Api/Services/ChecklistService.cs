using Microsoft.EntityFrameworkCore;
using SafetyVault.Api.Data;
using SafetyVault.Api.Entities;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public class ChecklistService : IChecklistService
    {

        //AppDbContext über Konstruktor-Injection übergeben, um auf die Datenbank zuzugreifen
        private readonly AppDbContext _context;

        public ChecklistService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<AddChecklistItemResult> AddItemAsync(int userId, int checklistId, CreateChecklistItemDto dto)
        {
            var checklistExists = await _context.Checklists.AnyAsync(c => c.Id == checklistId && c.UserId == userId);
            if (!checklistExists)
            {
                return new AddChecklistItemResult(AddChecklistItemStatus.ChecklistNotFound, null);
            }
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == dto.ItemId && i.UserId == userId);
            if (item is null)
            {
                return new AddChecklistItemResult(AddChecklistItemStatus.ItemNotFound, null);
            }
            var entity = new ChecklistItem
            {
                ChecklistId = checklistId,
                ItemId = dto.ItemId,
                TargetQuantity = dto.TargetQuantity,
                ActualQuantity = 0,
                PackedAt = null
            };
            _context.ChecklistItems.Add(entity);
            await _context.SaveChangesAsync();
            return new AddChecklistItemResult(AddChecklistItemStatus.Success, new ChecklistItemDto
            {
                Id = entity.Id,
                Name = item.Name,
                Description = item.Description,
                ChecklistId = entity.ChecklistId,
                TargetQuantity = entity.TargetQuantity,
                ActualQuantity = entity.ActualQuantity,
                PackedAt = entity.PackedAt,
                ItemId = entity.ItemId
            });
        }
        public async Task<IEnumerable<ChecklistSummaryDto>> GetAllAsync(int userId)
        {
            var checklistSummaries = await _context.Checklists
                .Where(c => c.UserId == userId)
                .Select(c => new ChecklistSummaryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ItemCount = c.ChecklistItems.Count()
                })
                .ToListAsync();
            return checklistSummaries;
        }
        public async Task<ChecklistDto?> GetByIdAsync(int userId, int id)
        {
            var checklist = await _context.Checklists
                .Where(c => c.UserId == userId && c.Id == id)
                .Select(c => new ChecklistDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Items = c.ChecklistItems.Select(ci => new ChecklistItemDto
                    {
                        Id = ci.Id,
                        Name = ci.Item.Name,
                        Description = ci.Item.Description,
                        ChecklistId = ci.ChecklistId,
                        TargetQuantity = ci.TargetQuantity,
                        ActualQuantity = ci.ActualQuantity,
                        PackedAt = ci.PackedAt,
                        ItemId = ci.ItemId
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            return checklist;
        }
            
        public async Task<ChecklistItemDto?> GetItemAsync(int userId, int checklistId, int checklistItemId)
        {
            var item = await _context.ChecklistItems
                .Where(ci => ci.ChecklistId == checklistId && ci.Id == checklistItemId && ci.Checklist.UserId == userId)
                .Select(ci => new ChecklistItemDto
                {
                    Id = ci.Id,
                    Name = ci.Item.Name,
                    Description = ci.Item.Description,
                    ChecklistId = ci.ChecklistId,
                    TargetQuantity = ci.TargetQuantity,
                    ActualQuantity = ci.ActualQuantity,
                    PackedAt = ci.PackedAt,
                    ItemId = ci.ItemId
                })
                .FirstOrDefaultAsync();
            return item;
        }
        public async Task<ChecklistItemDto?> UpdateItemAsync(int userId, int checklistId, int checklistItemId, UpdateChecklistItemDto dto)
        {
            var entry = await _context.ChecklistItems
                .Where(ci => ci.ChecklistId == checklistId && ci.Id == checklistItemId && ci.Checklist.UserId == userId)
                .FirstOrDefaultAsync();
            if (entry is null)
            {
                return null;
            }
            entry.TargetQuantity = dto.TargetQuantity;
            entry.ActualQuantity = dto.ActualQuantity;

            var isPacked = entry.ActualQuantity >= entry.TargetQuantity;

            if (isPacked && entry.PackedAt is null)
            {
                entry.PackedAt = DateTimeOffset.UtcNow;
            }
            else if (!isPacked)
            {
                entry.PackedAt = null;
            }

            await _context.SaveChangesAsync();
            return await GetItemAsync(userId, checklistId, checklistItemId);
        }

        public async Task<bool> DeleteItemAsync(int userId, int checklistId, int checklistItemId)
        {
            var entry = await _context.ChecklistItems
                .Where(ci => ci.ChecklistId == checklistId && ci.Id == checklistItemId && ci.Checklist.UserId == userId)
                .FirstOrDefaultAsync();
            if (entry is null)
            {
                return false;
            }
            _context.ChecklistItems.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CreateChecklistResult> CreateAsync(int userId, CreateChecklistDto dto)
        {
            var nameExists = await _context.Checklists.AnyAsync(c => c.UserId == userId && c.Name == dto.Name);
            if (nameExists)
            {
                return new CreateChecklistResult(CreateChecklistStatus.NameAlreadyExists, null);
            }
            var entity = new Checklist
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description
            };
            _context.Checklists.Add(entity);
            await _context.SaveChangesAsync();
            var checklistDto = new ChecklistDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Items = new List<ChecklistItemDto>()
            };
            return new CreateChecklistResult(CreateChecklistStatus.Success, checklistDto);
        }
    }
}
