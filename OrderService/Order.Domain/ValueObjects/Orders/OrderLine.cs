using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.ValueObjects.Orders
{
    public sealed record OrderLine
    {
        public Sku Sku { get; init; }
        public Quantity Quantity { get; init; }

        public OrderLine(Sku sku, Quantity quantity)
        {
            Validate(sku, quantity);
            Sku = sku;
            Quantity = quantity;
        }

        private static void Validate(Sku sku, Quantity quantity)
        {
            DomainValidationException.When(sku is null, "SKU is required.");
            DomainValidationException.When(quantity is null, "Quantity is required.");
        }
    }
}
