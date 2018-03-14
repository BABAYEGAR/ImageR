using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;

namespace CamerackStudio.Models.APIFactory
{
    public class ImageFactory
    {
        public async Task<List<ImageDownload>> GetAllDownloads(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            IEnumerable<ImageDownload> downloads = new List<ImageDownload>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    var httpResponse = await httpClient.GetAsync(baseAddress);
                    if (httpResponse != null)
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                            downloads = await Task.Run(() => JsonConvert.DeserializeObject<List<ImageDownload>>(stringData));
                        }
                }
                return downloads.ToList();
            }
            catch (Exception)
            {
                return downloads.ToList();
            }
        }
     
    }
}