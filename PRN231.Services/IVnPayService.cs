using Microsoft.AspNetCore.Http;
using PRN231.Models.DTOs.Request;

namespace PRN231.Services
{
    public interface IVnPayService
    {
        string CreatePayment(CreatePaymentRequest createPaymentRequest);

        string CreatePaymentForUserCredit(CreatePaymentUserRequest createPaymentRequest);

        int GetPaymentResult();
    }
}
