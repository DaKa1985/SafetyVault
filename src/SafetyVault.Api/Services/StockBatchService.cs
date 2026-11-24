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
        public async Task<IEnumerable<StockBatchDto>> GetAllAsync(int userId)
        {
            var stockBatches = await _context.StockBatches
                .Where(sb => sb.Quantity > 0 && sb.Item.UserId == userId)
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

        public async Task<StockBatchDto?> GetByIdAsync(int userId, int id)
        {
            var stockBatch = await _context.StockBatches
                .Where(sb => sb.Id == id && sb.Item.UserId == userId)
                .Select(ToDto)
                .FirstOrDefaultAsync();
            return stockBatch;
        }

        public async Task<BookInStockBatchResult> BookInAsync(int userId, BookInStockBatchDto dto)
        {
            var itemExists = await _context.Items.AnyAsync(i => i.Id == dto.ItemId && i.UserId == userId);
            if (!itemExists)
            {
                return new BookInStockBatchResult(BookInStockBatchStatus.ItemNotFound, null);
            }
            var stockBatch = await _context.StockBatches
                .FirstOrDefaultAsync(sb => sb.ItemId == dto.ItemId && sb.BestBefore == dto.BestBefore && sb.Item.UserId == userId);
            if (stockBatch is not null)
            {
                stockBatch.Quantity += dto.Quantity;
                await _context.SaveChangesAsync();
                return new BookInStockBatchResult(BookInStockBatchStatus.Increased, await GetByIdAsync(userId, stockBatch.Id));
            }
            var newBatch = new StockBatch
            {
                ItemId = dto.ItemId,
                Quantity = dto.Quantity,
                BestBefore = dto.BestBefore
            };
            _context.StockBatches.Add(newBatch);
            await _context.SaveChangesAsync();
            return new BookInStockBatchResult(BookInStockBatchStatus.Created, await GetByIdAsync(userId, newBatch.Id));
        }

        public async Task<WithdrawStockBatchResult> WithdrawAsync(int userId, int stockBatchId, WithdrawStockBatchDto dto)
        {
            var stockBatch = await _context.StockBatches.FirstOrDefaultAsync(sb => sb.Id == stockBatchId && sb.Item.UserId == userId); 
            if (stockBatch is null)
            {
                return new WithdrawStockBatchResult(WithdrawStockBatchStatus.BatchNotFound, null);
            }
            var newQuantity = stockBatch.Quantity - dto.Quantity;
            if (newQuantity < 0)
            {
                return new WithdrawStockBatchResult(WithdrawStockBatchStatus.InsufficientQuantity, null);
            }
            stockBatch.Quantity = newQuantity;
            await _context.SaveChangesAsync();
            return new WithdrawStockBatchResult(WithdrawStockBatchStatus.Success, await GetByIdAsync(userId, stockBatch.Id));
        }
    }
}