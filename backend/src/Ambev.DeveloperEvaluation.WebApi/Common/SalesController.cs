using Ambev.DeveloperEvaluation.Application.SalesRequest;
using Ambev.DeveloperEvaluation.Application.SalesResponse;
using Ambev.DeveloperEvaluation.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSaleRequest request)
        {
            try
            {
                var sale = await _salesService.CreateSaleAsync(request);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sale = await _salesService.GetSaleByIdAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SaleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var sales = await _salesService.GetAllSalesAsync();
            return Ok(sales);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSaleRequest request)
        {
            var updated = await _salesService.UpdateSaleAsync(id, request);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _salesService.CancelSaleAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { message = "Venda cancelada com sucesso." });
        }
    }
}
