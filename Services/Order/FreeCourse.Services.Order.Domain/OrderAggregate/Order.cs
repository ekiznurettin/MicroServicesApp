using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }

        private readonly List<OrderItem> _orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public Order()
        {

        }
        public Order(Address address, string buyerId)
        {
            CreatedDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
            _orderItems = new List<OrderItem>();
        }

        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl, int quaninty)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);
            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price, quaninty);
                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price * x.Quaninty);
    }
}
