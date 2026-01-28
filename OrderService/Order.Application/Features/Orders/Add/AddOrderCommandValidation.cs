using FluentValidation;
using Order.Application.Dto.Orders;

namespace Order.Application.Features.Orders.Add
{
   
    internal sealed class AddOrderCommandValidation : AbstractValidator<AddOrderCommand>
    {
        public AddOrderCommandValidation()
        {
            RuleFor(x => x.Payload)
                .NotNull()
                .WithMessage("Payload is required.");

            When(x => x.Payload is not null, () =>
            {
                RuleFor(x => x.Payload.Lines)
                    .NotNull().WithMessage("Lines are required.")
                    .NotEmpty().WithMessage("At least one order line is required.");

                RuleForEach(x => x.Payload.Lines).ChildRules(line =>
                {
                    line.RuleFor(l => l.Sku)
                        .NotEmpty().WithMessage("SKU is required.")
                        .MaximumLength(64).WithMessage("SKU is too large.");

                    line.RuleFor(l => l.Quantity)
                        .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                        .LessThanOrEqualTo(1_000_000).WithMessage("Quantity is too large.");
                });
            });
        }
    }
}
