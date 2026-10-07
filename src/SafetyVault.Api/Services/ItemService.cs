using Microsoft.EntityFrameworkCore;
using SafetyVault.Api.Data;
using SafetyVault.Api.Entities;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;
        public ItemService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await _context.Items
                .OrderBy(i => i.Name)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description
                })
                .ToListAsync();
            return items;
        }
        public async Task<ItemDto?> GetByIdAsync(int id)
        {
            var item = await _context.Items
                .Where(i => i.Id == id)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description
                })
                .FirstOrDefaultAsync();
            return item;
        }
        public async Task<CreateItemResult> CreateAsync(CreateItemDto dto)
        {
            var nameAlreadyExists = await _context.Items.AnyAsync(i => i.Name == dto.Name);
            if (nameAlreadyExists)
            {
                return new CreateItemResult(CreateItemStatus.NameAlreadyExists, null);
            }

            var entity = new Item
            {
                Name = dto.Name,
                Description = dto.Description
            };
            _context.Items.Add(entity);
            await _context.SaveChangesAsync();
            return new CreateItemResult(CreateItemStatus.Success, new ItemDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            });
        }
    }
}
