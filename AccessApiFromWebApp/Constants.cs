using System.Net.Http;

namespace OktaWebApp
{
    public class Constants
    {
        public static HttpClient RestClient { get; private set; }

        static Constants()
        {
            RestClient = new HttpClient();
            RestClient.DefaultRequestHeaders.UserAgent.ParseAdd("Agent/1.0");
            RestClient.DefaultRequestHeaders.Accept.Clear();
        }
                
    }
}
