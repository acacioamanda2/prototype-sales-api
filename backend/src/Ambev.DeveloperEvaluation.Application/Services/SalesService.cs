using Ambev.DeveloperEvaluation.Application.SalesRequest;
using Ambev.DeveloperEvaluation.Application.SalesResponse;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class SalesService : ISalesService
    {
        private readonly ISaleRepository _repository;
        private readonly ILogger<SalesService> _logger;

        public SalesService(ISaleRepository repository, ILogger<SalesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SaleDto> CreateSaleAsync(CreateSaleRequest request)
        {
            var entity = new SaleEntity
            {
                Id = Guid.NewGuid(),
                SaleNumber = request.SaleNumber,
                SaleDate = request.SaleDate,
                Customer = request.Customer,
                Branch = request.Branch,
                IsCancelled = false,
                Items = request.Items.Select(item => CreateItem(item.ProductId, item.ProductDescription, item.Quantity, item.UnitPrice)).ToList()
            };

            entity.TotalAmount = entity.Items.Sum(i => i.Total);

            await _repository.AddAsync(entity);
            _logger.LogInformation("Event: SaleCreated - Sale ID: {SaleId}", entity.Id);

            return MapToDto(entity);
        }

        public async Task<SaleDto> UpdateSaleAsync(Guid id, UpdateSaleRequest request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null || entity.IsCancelled)
                throw new Exception("Sale not found or already cancelled.");

            if (!string.IsNullOrWhiteSpace(request.Customer))
                entity.Customer = request.Customer;

            if (!string.IsNullOrWhiteSpace(request.Branch))
                entity.Branch = request.Branch;

            if (request.Items != null)
            {
                entity.Items.Clear();
                entity.Items.AddRange(request.Items.Select(item => CreateItem(item.ProductId, item.ProductDescription, item.Quantity, item.UnitPrice)));
            }

            entity.TotalAmount = entity.Items.Sum(i => i.Total);

            await _repository.UpdateAsync(entity);
            _logger.LogInformation("Event: SaleModified - Sale ID: {SaleId}", entity.Id);

            return MapToDto(entity);
        }

        public async Task<bool> CancelSaleAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null || entity.IsCancelled)
                return false;

            entity.IsCancelled = true;
            await _repository.UpdateAsync(entity);
            _logger.LogInformation("Event: SaleCancelled - Sale ID: {SaleId}", entity.Id);

            return true;
        }

        public async Task<SaleDto?> GetSaleByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDto);
        }

        private SaleItemEntity CreateItem(Guid productId, string description, int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new InvalidOperationException($"It is not allowed to sell more than 20 units of the product '{description}'.");

            decimal discount = CalculateDiscount(quantity, unitPrice);
            decimal total = (quantity * unitPrice) - discount;

            return new SaleItemEntity
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                ProductDescription = description,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Discount = discount,
                Total = total
            };
        }

        private decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity >= 10)
                return quantity * unitPrice * 0.2m;
            if (quantity >= 4)
                return quantity * unitPrice * 0.1m;
            return 0;
        }

        private static SaleDto MapToDto(SaleEntity sale)
        {
            return new SaleDto
            {
                Id = sale.Id!.Value,
                SaleNumber = sale.SaleNumber,
                SaleDate = sale.SaleDate,
                Customer = sale.Customer,
                Branch = sale.Branch,
                TotalAmount = sale.TotalAmount,
                IsCancelled = sale.IsCancelled,
                Items = sale.Items.Select(i => new SaleItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductDescription = i.ProductDescription,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    Total = i.Total
                }).ToList()
            };
        }
    }
}
