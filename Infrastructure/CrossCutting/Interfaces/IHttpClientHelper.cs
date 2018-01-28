using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infrastructure.CrossCutting.Interfaces
{
    public interface IHttpClientHelper
    {
        HttpResponseMessage PostAsync(string url, HttpContent content, AuthenticationHeaderValue bearerMktPlaceToken = null);
        HttpResponseMessage SendAsync(AuthenticationHeaderValue bearerMktPlaceToken, HttpRequestMessage request);       

        HttpResponseMessage GetAsync(string url, AuthenticationHeaderValue bearerMktPlaceToken = null);
        HttpResponseMessage GetAsyncSoap(string url, AuthenticationHeaderValue bearerMktPlaceToken = null);        
    }
}
