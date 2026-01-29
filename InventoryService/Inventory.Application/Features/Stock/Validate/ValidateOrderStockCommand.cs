using Inventory.Application.Dto.Stock.Validate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Stock.Validate
{
    public sealed record ValidateOrderStockCommand(
         Guid OrderId,
         IReadOnlyList<ValidateOrderStockLineDto> Lines
     ) : IRequest<ValidateOrderStockResultDto>;

}
