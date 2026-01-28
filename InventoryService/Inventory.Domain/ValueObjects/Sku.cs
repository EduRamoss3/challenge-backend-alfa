using Inventory.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.ValueObjects
{
    public sealed record Sku
    {
        public string Value { get; init; }

        public Sku(string value)
        {
            Validate(value);
            Value = value;
        }

        private static void Validate(string value)
        {
            DomainValidationException.When(string.IsNullOrWhiteSpace(value), "SKU is required.");
            DomainValidationException.When(value.Length > 64, "SKU is too large (max 64).");
            DomainValidationException.ValidateInvalidCharacters(value);
        }
    }
}
