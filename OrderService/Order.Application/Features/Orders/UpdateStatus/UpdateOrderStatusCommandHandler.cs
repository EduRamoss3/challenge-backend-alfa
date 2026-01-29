using MediatR;
using Order.Domain.Exceptions;
using Order.Domain.Interfaces.UnitOfWork;
using Order.Domain.ValueObjects.Orders;

namespace Order.Application.Features.Orders.UpdateStatus
{
    public sealed class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateOrderStatusCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken ct)
        {
            DomainValidationException.When(request.OrderId == Guid.Empty, "OrderId is required.");

            var order = await _uow.Orders.GetByIdForUpdateAsync(request.OrderId, ct);
            if (order is null) return; 

            if (request.Approved)
            {
                order.MarkApproved();
            }
            else
            {
                DomainValidationException.When(request.Failures is null || request.Failures.Count == 0,
                    "Failures are required when marking OUT_OF_STOCK.");

                var failures = request?.Failures!.Select(f =>
                    new StockFailure(new Sku(f.Sku), f.Requested, f.Available));

                order.MarkOutOfStock(failures);
            }

            await _uow.CommitAsync(ct);
        }
    }
}
