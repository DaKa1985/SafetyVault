using Microsoft.EntityFrameworkCore;
using SafetyVault.Api.Data;
using SafetyVault.Api.Entities;
using SafetyVault.Shared.Models;
using System.Linq.Expressions;

namespace SafetyVault.Api.Services
{
    public class StockBatchService : IStockBatchService
    {
        private readonly AppDbContext _context;
        public StockBatchService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<StockBatchDto>> GetAllAsync()
        {
            var stockBatches = await _context.StockBatches
                .Where(sb => sb.Quantity > 0)
                .OrderBy(sb => sb.BestBefore == null)
                .ThenBy(sb => sb.BestBefore)
                .ThenBy(sb => sb.Item.Name)
                .Select(ToDto)
                .ToListAsync();
            return stockBatches;
        }

        private static readonly Expression<Func<StockBatch, StockBatchDto>> ToDto = sb => new StockBatchDto
        {
            Id = sb.Id,
            ItemId = sb.ItemId,
            ItemName = sb.Item.Name,
            Quantity = sb.Quantity,
            BestBefore = sb.BestBefore
        };

        public async Task<StockBatchDto?> GetByIdAsync(int id)
        {
            var stockBatch = await _context.StockBatches
                .Where(sb => sb.Id == id)
                .Select(ToDto)
                .FirstOrDefaultAsync();
            return stockBatch;
        }
    }
}
