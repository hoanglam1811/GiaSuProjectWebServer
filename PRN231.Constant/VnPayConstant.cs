using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Constant
{
    public class VnPayConstant
    {
        public const string VERSION = "2.1.0";

        public const string PAY_COMMAND = "pay";

        public const string TMN_CODE = "B8R9R57L";

        public const string HASH_SECRET = "7MHDEW898EWMGPOFB835OZDNKUV81LGU";

        public const string CURRENCY_CODE = "VND";

        public const string LOCALE = "vn";

        public const string ORDER_TYPE = "other";

        public const string PAY_URL = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";

        public const string RETURN_URL = "http://localhost:5176/api/VnPay/Result";
    }
}
