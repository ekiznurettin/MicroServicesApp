using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class OrderItem:Entity
    {
        public string ProductId { get; private set; }
        public string ProductName{ get; private set; }
        public string PictureUrl{ get; private set; }
        public decimal Price{ get; private set; }
        public int Quaninty{ get; private set; }
        public OrderItem()
        {

        }
        public OrderItem(string productId, string productName, string pictureUrl, decimal price, int quaninty)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            Quaninty = quaninty;
        }

        public void UpdateOrderItem(string productName, string pictureUrl, decimal price, int quaninty)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            Quaninty = quaninty;
        }
    }
}
