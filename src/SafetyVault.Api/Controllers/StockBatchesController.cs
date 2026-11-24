using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StockBatchesController : ControllerBase
    {
        private readonly IStockBatchService _stockBatchService;

        public StockBatchesController(IStockBatchService stockBatchService)
        {
            _stockBatchService = stockBatchService ?? throw new ArgumentNullException(nameof(stockBatchService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockBatchDto>>> GetAll()
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
            var stockBatches = await _stockBatchService.GetAllAsync(userId);
            return stockBatches.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockBatchDto>> GetById(int id)
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
            var stockBatch = await _stockBatchService.GetByIdAsync(userId, id);
            if (stockBatch is null)
            {
                return NotFound();
            }
            return stockBatch;
        }

        [HttpPost]
        public async Task<ActionResult<StockBatchDto>> BookIn(BookInStockBatchDto dto)
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
            var result = await _stockBatchService.BookInAsync(userId, dto);
            switch (result.Status)
            {
                case BookInStockBatchStatus.Created:
                    return CreatedAtAction(nameof(GetById), new { id = result.StockBatch!.Id }, result.StockBatch);
                case BookInStockBatchStatus.Increased:
                    return Ok(result.StockBatch);
                case BookInStockBatchStatus.ItemNotFound:
                    return BadRequest($"Item with ID '{dto.ItemId}' not found.");
                default:
                    throw new InvalidOperationException($"Unexpected status: {result.Status}");
            }
        }

        [HttpPost("{id}/withdraw")]
        public async Task<ActionResult<StockBatchDto>> Withdraw(int id, WithdrawStockBatchDto dto)
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? throw new InvalidOperationException("User ID not found in token."));
            var result = await _stockBatchService.WithdrawAsync(userId, id, dto);
            switch (result.Status)
            {
                case WithdrawStockBatchStatus.Success:
                    return Ok(result.StockBatch);
                case WithdrawStockBatchStatus.BatchNotFound:
                    return NotFound();
                case WithdrawStockBatchStatus.InsufficientQuantity:
                    return Conflict($"Cannot withdraw {dto.Quantity} items from stock batch with ID '{id}'. Not enough in stock.");
                default:
                    throw new InvalidOperationException($"Unexpected status: {result.Status}");
            }
        }
    }
}
