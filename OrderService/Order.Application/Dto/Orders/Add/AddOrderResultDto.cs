using Order.Domain.Enums.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dto.Orders.Add
{
    public sealed record AddOrderResultDto
    {
        public Guid OrderId { get; init; }
        public Status Status { get; init; }  
    }
}
