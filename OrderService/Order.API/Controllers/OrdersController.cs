using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dto.Orders.Add;
using Order.Application.Features.Orders.Add;

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
        [ProducesResponseType(typeof(AddOrderResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddOrderResultDto>> Create([FromBody] AddOrderDto dto, CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new AddOrderCommand(dto), ct);

                // 201 + Location header + response body
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.OrderId, version = "1.0" },
                    result
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            return Ok(new { Id = id });
        }
    }
}
