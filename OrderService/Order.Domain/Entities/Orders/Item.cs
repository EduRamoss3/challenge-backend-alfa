using Order.Domain.Entities.Base;
using Order.Domain.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities.Orders
{
    public sealed class Item : Entity
    {
        public string Name { get; private set; } 
        public string Description { get; private set; } 

        public static void Validate(string name, string description)
        {
            DomainValidationException.When(string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name), "The name of item is required!");
            DomainValidationException.When(name.Length > 300, $"The max number of characters for this property '{nameof(name)}' is of 300 characters");
            DomainValidationException.ValidateInvalidCharacters(name);

            DomainValidationException.When(string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description), "The description of the item is required!");
            DomainValidationException.When(description.Length > 300, $"The max number of characters for this property  {nameof(description)}  is of 300 characters");
            DomainValidationException.ValidateInvalidCharacters(description);
        }

        public Item(string name, string description)
        {
            Validate(name, description);
            Name = name;
            Description = description;
        }
    }
}
