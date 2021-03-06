﻿using System;
using System.Net.Http;

namespace CamerackStudio.Models.APIFactory
{
    public class ImageUploadFactory
    {
        public async void UploadImage(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    await httpClient.GetAsync(baseAddress);
                }
            }
            catch (Exception)
            {
            }
        }
        public async void ImageActionMessage(string baseAddress)
        {
            if (baseAddress == null) throw new ArgumentNullException(nameof(baseAddress));
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Do the actual request and await the response
                    await httpClient.GetAsync(baseAddress);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}