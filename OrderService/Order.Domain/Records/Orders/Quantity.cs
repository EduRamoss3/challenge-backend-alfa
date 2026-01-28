using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Records.Orders
{
    public sealed record Quantity
    {
        public int Value { get; init; }

        public Quantity(int value)
        {
            Validate(value);
            Value = value;
        }

        private static void Validate(int value)
        {
            DomainValidationException.When(value <= 0, "Quantity must be greater than zero.");
            DomainValidationException.When(value > 1_000_000, "Quantity is too large.");
        }
    }
}
