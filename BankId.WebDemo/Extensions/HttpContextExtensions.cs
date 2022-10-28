using System.Net;

namespace BankId.WebDemo.Extensions
{
    public static class HttpContextExtensions
    {
        private static string _localIp { get; set; }

        public static async Task<string> GetClientIpAsync(this HttpContext context)
        {
            if (!IsLocal(context))
            {
                return context.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                if (_localIp == null)
                {
                    var httpClient = new HttpClient();
                    _localIp = await httpClient.GetStringAsync("https://api.ipify.org");
                }
                return _localIp;
            }
        }
        public static bool IsLocal(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress) ||
                IPAddress.IsLoopback(context.Connection.RemoteIpAddress);
        }
    }
}
