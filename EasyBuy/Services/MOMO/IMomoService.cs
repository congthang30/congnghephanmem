using EasyBuy.Models;
using EasyBuy.Models.MOMO;

namespace EasyBuy.Services.MOMO
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfo model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
