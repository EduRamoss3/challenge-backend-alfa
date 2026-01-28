using Order.Domain.Entities.Base;
using Order.Domain.Enums.Orders;
using Order.Domain.Exceptions;
using Order.Domain.Records.Orders;

namespace Order.Domain.Entities.Orders
{
    public sealed class Order : Entity
    {
        private readonly List<OrderLine> _lines = new();
        private readonly List<StockFailure> _stockFailures = new();

        public DateTime CreatedAt { get; private set; }
        public Status Status { get; private set; }

        public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();
        public IReadOnlyCollection<StockFailure> StockFailures => _stockFailures.AsReadOnly();

        #region Construtores
        public Order(OrderLine line) : this(new[] { line })
        {
        }
        // EF Core
        private Order()
        {
            CreatedAt = DateTime.UtcNow;
            Status = Status.PENDENTE;
        }
        public Order(IEnumerable<OrderLine> lines)
        {
            DomainValidationException.When(lines is null, "Order lines are required.");

            CreatedAt = DateTime.UtcNow;
            Status = Status.PENDENTE;

            foreach (var line in lines!)
                AddLine(line);

            DomainValidationException.When(_lines.Count == 0, "An order must contain at least one line.");
        }

        #endregion

        public void AddLine(OrderLine line)
        {
            DomainValidationException.When(line is null, "Order line is required.");
            DomainValidationException.When(Status != Status.PENDENTE, "You can only add lines when the order is PENDING.");

            var existing = _lines.FirstOrDefault(x => x.Sku.Value == line!.Sku.Value);

            if (existing is null)
            {
                _lines.Add(line!);
                return;
            }

            var summedQty = new Quantity(existing.Quantity.Value + line!.Quantity.Value);

            _lines.Remove(existing);
            _lines.Add(existing with { Quantity = summedQty });
        }

        public void MarkApproved()
        {
            DomainValidationException.When(Status != Status.PENDENTE, "Only a PENDING order can be approved.");
            DomainValidationException.When(_lines.Count == 0, "An order must contain at least one line.");

            _stockFailures.Clear();
            Status = Status.APROVADO;
        }

        public void MarkOutOfStock(IEnumerable<StockFailure> failures)
        {
            DomainValidationException.When(Status != Status.PENDENTE, "Only a PENDING order can be marked as OUT_OF_STOCK.");
            DomainValidationException.When(failures is null, "Failures list is required.");

            var list = failures!.ToList();
            DomainValidationException.When(list.Count == 0, "Failures list cannot be empty when marking OUT_OF_STOCK.");

            _stockFailures.Clear();
            _stockFailures.AddRange(list);

            Status = Status.SEM_ESTOQUE;
        }

      
    }
}
