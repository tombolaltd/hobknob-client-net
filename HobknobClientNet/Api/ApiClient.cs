using System;
using System.IO;
using System.Net;
using System.Text;
using HobknobClientNet.Api.Response;
using Newtonsoft.Json;

namespace HobknobClientNet.Api
{
    public class ApiClient
    {
        private readonly Uri _keysUri;

        public ApiClient(Uri keysUri)
        {
            _keysUri = keysUri;
        }

        public ApiResponse Get()
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(_keysUri);
            webRequest.Accept = "application/json";

            try
            {
                using (var webResponse = webRequest.GetResponse())
                {
                    return GetResponse(webResponse);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw;
            }
        }

        private static ApiResponse GetResponse(WebResponse webResponse)
        {
            string apiResponseJson;
            using (var responseStream = webResponse.GetResponseStream())
            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                apiResponseJson = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<ApiResponse>(apiResponseJson);
        }
    }
}
