using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Infrastructure.CrossCutting.Interfaces;

namespace Infrastructure.CrossCutting.Helper
{
    public class HttpClientHelper : IHttpClientHelper
    {
        public HttpResponseMessage PostAsync(string url, HttpContent content, AuthenticationHeaderValue bearerMktPlaceToken = null)
        {
            using (var client = new HttpClient())
            {
                if (bearerMktPlaceToken != null)
                    client.DefaultRequestHeaders.Authorization = bearerMktPlaceToken;

                return client.PostAsync(url, content).Result;
            }
        }

        public HttpResponseMessage SendAsync(AuthenticationHeaderValue bearerMktPlaceToken, HttpRequestMessage request)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = bearerMktPlaceToken;
                return client.SendAsync(request).Result;
            }
        }

        public HttpResponseMessage GetAsync(string url, AuthenticationHeaderValue bearerMktPlaceToken = null)
        {
            using (var client = new HttpClient())
            {
                //client.Timeout = TimeSpan.FromSeconds(3);

                if (bearerMktPlaceToken != null)
                    client.DefaultRequestHeaders.Authorization = bearerMktPlaceToken;

                return client.GetAsync(url).Result;
            }
        }

        public HttpResponseMessage GetAsyncSoap(string url, AuthenticationHeaderValue bearerMktPlaceToken = null)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(3);

                if (bearerMktPlaceToken != null)
                    client.DefaultRequestHeaders.Authorization = bearerMktPlaceToken;

                return client.GetAsync(url).Result;
            }
        }

    }
}
