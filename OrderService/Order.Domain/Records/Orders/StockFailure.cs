using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Records.Orders
{
    public sealed record StockFailure
    {
        public Sku Sku { get; init; }
        public int Requested { get; init; }
        public int Available { get; init; }

        public StockFailure(Sku sku, int requested, int available)
        {
            Validate(sku, requested, available);
            Sku = sku;
            Requested = requested;
            Available = available;
        }

        private static void Validate(Sku sku, int requested, int available)
        {
            DomainValidationException.When(sku is null, "SKU is required.");
            DomainValidationException.When(requested <= 0, "Requested quantity must be greater than zero.");
            DomainValidationException.When(available < 0, "Available quantity cannot be negative.");
        }
    }

}
