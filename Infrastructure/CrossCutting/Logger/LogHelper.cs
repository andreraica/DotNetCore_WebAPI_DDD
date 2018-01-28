using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using Infrastructure.CrossCutting.Helper;

namespace Infrastructure.CrossCutting.Logger
{
    public static class LogHelper
    {
        public static async Task<HttpResponseMessage> PostJsonToGrayLogAsync(dynamic log)
        {
            var json = JsonConvert.SerializeObject(log, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });

            var urlApi = AppSettings.Get<string>("UrlApi");
            var credentialsPass = AppSettings.Get<string>("CredentialsPass");

            HttpResponseMessage response;

            //var json = JsonConvert.SerializeObject(log);

            //var urlApi = Configuration.Configuration.UrlApi; //AppSettings.Get<string>("UrlApi");
            //var credentialsPass = Configuration.Configuration.CredentialsPass; //AppSettings.Get<string>("CredentialsPass");
            //var urlApi = "http://prdlogsrv.grupoltm.com.br:12201/gelf";
            //var credentialsPass = "admin:admin";


            //HttpResponseMessage response = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new StringContent(json);
                    request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var url = string.Format("{0}", urlApi);
                    var credentials = Encoding.ASCII.GetBytes(credentialsPass); //usuario e senha para autenticação na API
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

                    response = await httpClient.PostAsync(new Uri(url), request);

                    if (!response.IsSuccessStatusCode)
                    {
                        response.Content = new StringContent("Não foi possivel acessar o GrayLog",
                            Encoding.UTF8, "application/json");
                    }
                }
            }
            catch (Exception ex)
            {
                response = new HttpResponseMessage(); 
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("Não foi possivel acessar o GrayLog ex:" + ex.Message,
                    Encoding.UTF8, "application/json");
            }

            return response;
        }

        public static string PostJsonToGrayLogSync(dynamic log)
        {
            var json = JsonConvert.SerializeObject(log, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });

            var urlApi = AppSettings.Get<string>("UrlApi");
            var credentialsPass = AppSettings.Get<string>("CredentialsPass");

            HttpResponseMessage response = null;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new StringContent(json);
                    request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var url = string.Format("{0}", urlApi);
                    var credentials = Encoding.ASCII.GetBytes(credentialsPass); //usuario e senha para autenticação na API
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

                    //response = await httpClient.PostAsync(new Uri(url), request);
                    response = Task.Run(() => httpClient.PostAsync(new Uri(url), request)).Result;
                    //var response2 = httpClient.PostAsync(new Uri(url), request).Wait();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception(string.Format("Graylog error: StatusCode: {0}. Phrase: {1}.", response.StatusCode, response.ReasonPhrase));

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = new StringContent("Não foi possivel acessar o GrayLog ex:" + ex.Message,
                    Encoding.UTF8, "application/json");
            }

            return response.ToString();
        }


        public static Task<HttpResponseMessage> PostJsonToGrayLogInLineAsync(InlineLog log)
        {
            return PostJsonToGrayLogAsync(log);
        }

    }
}
