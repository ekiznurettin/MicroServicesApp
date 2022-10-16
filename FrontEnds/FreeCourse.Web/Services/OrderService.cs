using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayment;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInput)
        {
            var basket = await _basketService.Get();
            var payment = new PaymentInfoInput()
            {
                CardName = checkoutInput.CardName,
                CardNumber = checkoutInput.CardNumber,
                CVV = checkoutInput.CVV,
                Expiration = checkoutInput.Expiration,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentService.ReceivePayment(payment);
            if (!responsePayment)
            {
                return new OrderCreatedViewModel() { Error = "Ödeme Alınamadı", IsSuccessful = false };
            }
            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput
                {
                    Province = checkoutInput.Province,
                    District = checkoutInput.District,
                    Line=checkoutInput.Line,
                    Street = checkoutInput.Street,
                    ZipCode = checkoutInput.ZipCode
                }              
            };
            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CourseName,
                    Quaninty=x.Quantity
                };
                orderCreateInput.OrderItems.Add(orderItem);
            });
            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);
            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { Error = "Sipariş Oluşturulamadı", IsSuccessful = false };
            }
            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response< OrderCreatedViewModel>>();
            orderCreatedViewModel.IsSuccessful = true;
            await _basketService.Delete();
            return orderCreatedViewModel.Data;
        }


        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }

        public Task SuspendOrder(CheckoutInfoInput checkoutInput)
        {
            throw new NotImplementedException();
        }
    }
}
