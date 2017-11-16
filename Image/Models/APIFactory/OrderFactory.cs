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
    public class OrderFactory
    {
        public async Task<List<Order>> GetAllOrdersAsync(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            IEnumerable<Order> orders = new List<Order>();

            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.GetAsync(baseAddress);
                if (httpResponse != null)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                        orders = await Task.Run(() => JsonConvert.DeserializeObject<List<Order>>(stringData));
                    }
            }
            return orders.ToList();
        }
        public async Task<List<Payment>> GetAllPaymentsAsync(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            IEnumerable<Payment> payments = new List<Payment>();

            using (var httpClient = new HttpClient())
            {
                // Do the actual request and await the response
                var httpResponse = await httpClient.GetAsync(baseAddress);
                if (httpResponse != null)
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                        payments = await Task.Run(() => JsonConvert.DeserializeObject<List<Payment>>(stringData));
                    }
            }
            return payments.ToList();
        }
    }
}