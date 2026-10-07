using Microsoft.AspNetCore.Mvc;
using SafetyVault.Api.Services;
using SafetyVault.Shared.Models;

namespace SafetyVault.Api.Controllers
{
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
            var stockBatches = await _stockBatchService.GetAllAsync();
            return stockBatches.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockBatchDto>> GetById(int id)
        {
            var stockBatch = await _stockBatchService.GetByIdAsync(id);
            if (stockBatch is null)
            {
                return NotFound();
            }
            return stockBatch;
        }
    }
}
