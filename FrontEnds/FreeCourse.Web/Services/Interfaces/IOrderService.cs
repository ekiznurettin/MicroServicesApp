using FreeCourse.Web.Models.Orders;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInput);
        Task<List<OrderViewModel>> GetOrder();
    }
}
