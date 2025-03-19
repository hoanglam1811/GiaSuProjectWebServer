using System.Security.Cryptography;
using System.Text;

namespace PRN231.Services.Implementations;
public class OtpService
{
    public string GenerateOtp(int length = 6)
    {
        var random = new Random();
        var otp = new char[length];
        for (int i = 0; i < length; i++)
        {
            otp[i] = (char)('0' + random.Next(0, 10));
        }
        return new string(otp);
    }

    public string HashOtp(string otp)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
