using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Dto.Stock.Validate
{
    public sealed record ValidateOrderStockLineDto(string Sku, int Quantity);

}
