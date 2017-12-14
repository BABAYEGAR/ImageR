using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CamerackStudio.Models.Entities;
using Newtonsoft.Json;

namespace CamerackStudio.Models.APIFactory
{
    public class AppUserFactory
    {
        public async Task<List<AppUser>> GetAllUsers(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            IEnumerable<AppUser> users = new List<AppUser>();
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
                            users = await Task.Run(() => JsonConvert.DeserializeObject<List<AppUser>>(stringData));
                        }
                }
                return users.ToList();
            }
            catch (Exception)
            {
                return users.ToList();
            }
        }
        public async Task<List<AppUserAccessKey>> GetUsersAccessKey(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            IEnumerable<AppUserAccessKey> accessKeys = new List<AppUserAccessKey>();
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
                            accessKeys = await Task.Run(() => JsonConvert.DeserializeObject<List<AppUserAccessKey>>(stringData));
                        }
                }
                return accessKeys.ToList();
            }
            catch (Exception)
            {
                return accessKeys.ToList();
            }
        }
        public async Task<AppUserAccessKey> UpdatePasswordAccessKey(string baseAddress,long id)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            AppUserAccessKey accessKeys = new AppUserAccessKey();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    baseAddress = baseAddress + id;
                    var httpResponse = await httpClient.GetAsync(baseAddress);
                    if (httpResponse != null)
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                            accessKeys = await Task.Run(() => JsonConvert.DeserializeObject<AppUserAccessKey>(stringData));
                        }
                }
                return accessKeys;
            }
            catch (Exception)
            {
                return accessKeys;
            }
        }
        public async Task<AppUserAccessKey> UpdateAccountActivationAccessKey(string baseAddress, long id)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            AppUserAccessKey accessKeys = new AppUserAccessKey();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    baseAddress = baseAddress + id;
                    var httpResponse = await httpClient.GetAsync(baseAddress);
                    if (httpResponse != null)
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                            accessKeys = await Task.Run(() => JsonConvert.DeserializeObject<AppUserAccessKey>(stringData));
                        }
                }
                return accessKeys;
            }
            catch (Exception)
            {
                return accessKeys;
            }
        }
        public async Task<ActionResponse> ActivateUser(string baseAddress, long id)
        {
            var response = new ActionResponse();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    baseAddress = baseAddress + id;
                    var httpResponse = await httpClient.GetAsync(baseAddress);
                    if (httpResponse != null)
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                            response = await Task.Run(() => JsonConvert.DeserializeObject<ActionResponse>(stringData));
                        }
                }
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        public async Task<ActionResponse> DeactivateUser(string baseAddress, long id)
        {
            var response = new ActionResponse();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    baseAddress = baseAddress + id;
                    var httpResponse = await httpClient.GetAsync(baseAddress);
                    if (httpResponse != null)
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var stringData = httpResponse.Content.ReadAsStringAsync().Result;
                            response = await Task.Run(() => JsonConvert.DeserializeObject<ActionResponse>(stringData));
                        }
                }
                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        public async Task<ActionResponse> RegisterUser(string baseAddress, AppUser appUser)
        {
            var response = new ActionResponse();
            try
            {
                // Serialize our concrete class into a JSON String
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(appUser));

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
            catch (Exception)
            {
                return response;
            }
        }

        public async Task<ActionResponse> EditProfile(string baseAddress, AppUser appUser)
        {
            var response = new ActionResponse();
            try
            {
                // Serialize our concrete class into a JSON String
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(appUser));

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
            catch (Exception)
            {
                return response;
            }
        }

        public async Task<ActionResponse> ChangePassword(string baseAddress, AppUser appUser)
        {
            var response = new ActionResponse();
            try
            {
                // Serialize our concrete class into a JSON String
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(appUser));

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
            catch (Exception)
            {
                return response;
            }
        }

        public async Task<ActionResponse> ForgetPasswordLink(string baseAddress, AccountModel model)
        {
            var response = new ActionResponse();
            try
            {
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
            catch (Exception)
            {
                return response;
            }
        }

        public async Task<ActionResponse> PasswordReset(string baseAddress, AccountModel model)
        {
            var response = new ActionResponse();
            try
            {
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
            catch (Exception)
            {
                return response;
            }
        }
    }
}