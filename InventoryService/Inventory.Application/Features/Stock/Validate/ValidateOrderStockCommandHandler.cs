using FluentValidation;
using Inventory.Application.Dto.Stock.Validate;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Stock.Validate
{
    internal sealed class ValidateOrderStockCommandHandler
         : IRequestHandler<ValidateOrderStockCommand, ValidateOrderStockResultDto>
    {
        private readonly IInventoryItemStockRepository _repo;
        private readonly IValidator<ValidateOrderStockCommand> _validator;

        public ValidateOrderStockCommandHandler(
            IValidator<ValidateOrderStockCommand> validator,
            IInventoryItemStockRepository repo)
        {
            _validator = validator;
            _repo = repo;
        }
        public async Task<ValidateOrderStockResultDto> Handle(ValidateOrderStockCommand request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var failures = new List<StockFailureResultDto>();

            foreach (var line in request.Lines)
            {
                DomainValidationException.When(string.IsNullOrWhiteSpace(line.Sku), "SKU is required.");
                DomainValidationException.When(line.Quantity <= 0, "Quantity must be greater than zero.");

                var stock = await _repo.GetBySkuAsync(line.Sku, ct);
                var available = stock?.Available ?? 0; 

                if (available < line.Quantity)
                {
                    failures.Add(new StockFailureResultDto(line.Sku, line.Quantity, available));
                }
            }

            return new ValidateOrderStockResultDto(
                request.OrderId,
                Approved: failures.Count == 0,
                Failures: failures
            );
        }
    }
}
