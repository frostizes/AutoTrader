using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Util.ExceptionsHandler;

namespace ServiceAccessLayer
{
    public class ServiceCaller
    {
        public static async Task<List<T>> ExecuteExternalServiceRequest<T>(string url, HttpClient client)
        {
            var result = new List<T>();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                result = DeserializeJson<T>(stringResponse);
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);
            }
            return result;
        }

        private static List<T> DeserializeJson<T>(string json)
        {
            try
            {
                List<T> models = JsonConvert.DeserializeObject<List<T>>(json,
                    new JsonSerializerSettings()
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace
                    });
                return models;
            }
            catch (Exception e)
            {
                throw new GenericException("error deserializing : " + e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
