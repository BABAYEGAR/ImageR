using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;

namespace CamerackStudio.Models.APIFactory
{
    public class OrderFactory
    {
        public async Task<List<Order>> GetAllOrdersAsync(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            IEnumerable<Order> orders = new List<Order>();
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
                            orders = await Task.Run(() => JsonConvert.DeserializeObject<List<Order>>(stringData));
                        }
                }
                return orders.ToList();
            }
            catch (Exception)
            {
                return orders.ToList();
            }
        }
        public async Task<List<Payment>> GetAllPaymentsAsync(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            IEnumerable<Payment> payments = new List<Payment>();
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
                            payments = await Task.Run(() => JsonConvert.DeserializeObject<List<Payment>>(stringData));
                        }
                }
                return payments.ToList();
            }
            catch (Exception)
            {
                return payments.ToList();
            }
          
        }
        public async Task<Payment>ApprovePaymentsAsync(string baseAddress,long id)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            Payment payment = new Payment();
            try
            {
                baseAddress = baseAddress + id;
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    var httpResponse = await httpClient.GetAsync(baseAddress);
                    if (httpResponse != null)
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                            payment = await Task.Run(() => JsonConvert.DeserializeObject<Payment>(stringData));
                        }
                }
                return payment;
            }
            catch (Exception)
            {
                return payment;
            }

        }
    }
}