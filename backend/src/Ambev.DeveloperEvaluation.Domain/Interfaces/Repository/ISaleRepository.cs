using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces.Repository
{
    public interface ISaleRepository
    {
        Task<SaleEntity> GetByIdAsync(Guid id);
        Task<List<SaleEntity>> GetAllAsync();
        Task AddAsync(SaleEntity sale);
        Task UpdateAsync(SaleEntity sale);
        Task DeleteAsync(SaleEntity sale);
    }
}
