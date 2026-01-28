using Order.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Records.Orders
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
            DomainValidationException.When(string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value),"SKU é obrigatório.");
            DomainValidationException.When(value.Length > 64,"SKU too large");
        }
    }
}

