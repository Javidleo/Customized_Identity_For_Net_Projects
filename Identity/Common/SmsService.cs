using System.Net;

namespace Identity.Common
{
    public class SmsService
    {
        public static void Send(string phoneNumber, string code)
        {
            WebClient client = new WebClient();
            string url = "";

            var content = client.DownloadString(url);
        }
    }
}
