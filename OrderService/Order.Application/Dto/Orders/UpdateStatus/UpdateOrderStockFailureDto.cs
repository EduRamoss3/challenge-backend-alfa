using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dto.Orders.UpdateStatus
{
    public sealed record UpdateOrderStockFailureDto(string Sku, int Requested, int Available);
}
