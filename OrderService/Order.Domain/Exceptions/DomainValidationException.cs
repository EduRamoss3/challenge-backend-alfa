using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Exceptions
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message) { }

        public static void When(bool condition, string message)
        {
            if (condition)
            {
                throw new DomainValidationException(message);
            }
        }
    }
}
