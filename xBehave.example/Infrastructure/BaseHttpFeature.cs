namespace xBehave.example.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public abstract class BaseHttpFeature
    {
        private readonly ISettings settings;
        private Cookie sessionCookie;

        public BaseHttpFeature()
        {
            settings = SettingsFactory.Create();
        }

        public bool Login(string emailAddress, string password)
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                CookieContainer cookieJar = new CookieContainer();
                handler.CookieContainer = cookieJar;
                client.BaseAddress = new Uri(settings.BaseUrl);
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("EmailAddress", emailAddress),
                    new KeyValuePair<string, string>("Password", password)
                });
                var result = client.PostAsync("/login", content).Result;
                IEnumerable<Cookie> responseCookies = cookieJar.GetCookies(client.BaseAddress).Cast<Cookie>();

                sessionCookie = responseCookies.First(x => x.Name == "Live.Session");
                return true;
            }
        }

        public HttpResponseMessage Get(string route)
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                CookieContainer cookieJar = new CookieContainer();
                handler.CookieContainer = cookieJar;
                cookieJar.Add(sessionCookie);
                client.BaseAddress = new Uri(settings.BaseUrl);
                
                return client.GetAsync(route).Result;
            }
        }

        public HttpResponseMessage GetJson(string route)
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                CookieContainer cookieJar = new CookieContainer();
                handler.CookieContainer = cookieJar;
                cookieJar.Add(sessionCookie);
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return client.GetAsync(route).Result;
            }
        }

        public HttpResponseMessage Post(string route, object body)
        {
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                CookieContainer cookieJar = new CookieContainer();
                handler.CookieContainer = cookieJar;
                cookieJar.Add(sessionCookie);
                client.BaseAddress = new Uri(settings.BaseUrl);

                return client.PostAsJsonAsync(route, JsonConvert.SerializeObject(body)).Result;
            }
        }
    }
}
