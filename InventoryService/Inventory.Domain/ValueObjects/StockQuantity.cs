using Inventory.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ValueObjects
{
    public sealed record StockQuantity
    {
        public int Value { get; init; }

        public StockQuantity(int value)
        {
            Validate(value);
            Value = value;
        }

        private static void Validate(int value)
        {
            DomainValidationException.When(value < 0, "Available stock cannot be negative.");
            DomainValidationException.When(value > 1_000_000_000, "Available stock is too large.");
        }
    }
}
