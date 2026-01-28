using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dto.Orders.Add;
using Order.Application.Dto.Orders.GetById;
using Order.Application.Features.Orders.Add;
using Order.Application.Features.Orders.GetById;
using Order.Domain.Exceptions;
using Swashbuckle.AspNetCore.Annotations;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new order with initial status PENDENTE.
        /// Inventory validation happens asynchronously.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates an order",
            Description = "Returns the order id and status"
        )]
        [ProducesResponseType(typeof(AddOrderResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddOrderResultDto>> Create([FromBody] AddOrderDto dto, CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new AddOrderCommand(dto), ct);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.OrderId, version = "1.0" },
                    result
                );
            }
            catch (DomainValidationException ex)
            {
                _logger.LogError(ex, "Domain validation error.");
                return Problem(
                    title: "Validation error",
                    detail: ex.Message,
                    statusCode: StatusCodes.Status422UnprocessableEntity
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating order.");
                return Problem(
                    title: "Unexpected error",
                    detail: "Order was not created. Please try again later.",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(
            Summary = "Get order by id",
            Description = "Returns the order details, including lines and stock validation failures (if any)."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Order found.", typeof(OrderDetailsDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order not found.")]
        public async Task<ActionResult<OrderDetailsDto>> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
