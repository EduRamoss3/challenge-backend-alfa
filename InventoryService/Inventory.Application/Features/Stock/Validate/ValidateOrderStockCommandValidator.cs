using FluentValidation;

namespace Inventory.Application.Features.Stock.Validate
{
    public sealed class ValidateOrderStockCommandValidator : AbstractValidator<ValidateOrderStockCommand>
    {
        public ValidateOrderStockCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("OrderId is required.");

            RuleFor(x => x.Lines)
                .NotNull()
                .WithMessage("Lines are required.")
                .Must(l => l.Count > 0)
                .WithMessage("At least one line is required.");

            RuleForEach(x => x.Lines).ChildRules(line =>
            {
                line.RuleFor(l => l.Sku)
                    .NotEmpty()
                    .WithMessage("SKU is required.")
                    .MaximumLength(64)
                    .WithMessage("SKU is too large (max 64).");

                line.RuleFor(l => l.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.")
                    .LessThanOrEqualTo(1_000_000)
                    .WithMessage("Quantity is too large.");
            });
        }
    }
}
