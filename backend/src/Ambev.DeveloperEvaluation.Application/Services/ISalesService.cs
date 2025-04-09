using Ambev.DeveloperEvaluation.Application.SalesRequest;
using Ambev.DeveloperEvaluation.Application.SalesResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public interface ISalesService
    {
        Task<SaleDto> CreateSaleAsync(CreateSaleRequest request);
        Task<SaleDto> UpdateSaleAsync(Guid id, UpdateSaleRequest request);
        Task<bool> CancelSaleAsync(Guid id);
        Task<SaleDto?> GetSaleByIdAsync(Guid id);
        Task<IEnumerable<SaleDto>> GetAllSalesAsync();
    }
}
