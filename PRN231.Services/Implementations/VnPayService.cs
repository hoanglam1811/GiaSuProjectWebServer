using Azure;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.X9;
using PRN231.Constant;
using PRN231.Models.DTOs.Request;
using PRN231.Services.Utils;

namespace PRN231.Services.Implementations
{
    public class VnPayService : IVnPayService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VnPayService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CreatePaymentForUserCredit(CreatePaymentUserRequest createPaymentRequest)
        {
            //Get Config Info
            string vnp_Returnurl = VnPayConstant.RETURN_URL;
            string vnp_Url = VnPayConstant.PAY_URL;
            string vnp_TmnCode = VnPayConstant.TMN_CODE;
            string vnp_HashSecret = VnPayConstant.HASH_SECRET;

            //Get payment input
            long orderId = DateTime.Now.Ticks;
            long amount = createPaymentRequest.Amount;
            DateTime createdDate = DateTime.Now;

            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayConstant.VERSION);
            vnpay.AddRequestData("vnp_Command", VnPayConstant.PAY_COMMAND);
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            
            vnpay.AddRequestData("vnp_CreateDate", createdDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", VnPayUtils.GetIpAddress(_httpContextAccessor));

            vnpay.AddRequestData("vnp_Locale", VnPayConstant.LOCALE);

            vnpay.AddRequestData("vnp_OrderInfo", createPaymentRequest.OrderInfo);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl+"/"+createPaymentRequest.UserId);
            vnpay.AddRequestData("vnp_TxnRef", orderId.ToString());

            //vnpay.AddRequestData("vnp_Bill_Email", createPaymentRequest.UserId.ToString());

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }

        public string CreatePayment(CreatePaymentRequest createPaymentRequest)
        {
            //Get Config Info
            string vnp_Returnurl = VnPayConstant.RETURN_URL;
            string vnp_Url = VnPayConstant.PAY_URL;
            string vnp_TmnCode = VnPayConstant.TMN_CODE;
            string vnp_HashSecret = VnPayConstant.HASH_SECRET;

            //Get payment input
            long orderId = DateTime.Now.Ticks;
            long amount = createPaymentRequest.Amount;
            DateTime createdDate = DateTime.Now;

            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayConstant.VERSION);
            vnpay.AddRequestData("vnp_Command", VnPayConstant.PAY_COMMAND);
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            
            vnpay.AddRequestData("vnp_CreateDate", createdDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", VnPayUtils.GetIpAddress(_httpContextAccessor));

            vnpay.AddRequestData("vnp_Locale", VnPayConstant.LOCALE);

            vnpay.AddRequestData("vnp_OrderInfo", createPaymentRequest.OrderInfo);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", orderId.ToString());

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }

        public int GetPaymentResult()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context.Request.Query.Count > 0)
            {
                string vnp_HashSecret = VnPayConstant.HASH_SECRET;
                var vnpayData = context.Request.Query;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData.Keys)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash =  context.Request.Query["vnp_SecureHash"];
                String TerminalID = context.Request.Query["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = context.Request.Query["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
