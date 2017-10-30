using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Image.Models.Entities;
using Newtonsoft.Json;

namespace Image.Models.APIFactory
{
    public class AuthenticateFactory
    {
        public async Task<ActionResponse> Login(string baseAddress, AccountModel model)
        {
            var response = new ActionResponse();
            // Serialize our concrete class into a JSON String
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(model));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.PostAsync(baseAddress, httpContent);
                if (httpResponse != null)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var stringData = httpResponse.Content.ReadAsStringAsync().Result;

                        //fetch logged in user
                        response = await Task.Run(() => JsonConvert.DeserializeObject<ActionResponse>(stringData));
                    }
            }
            return response;
        }
    }
}