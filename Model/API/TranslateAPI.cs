﻿using Azure;
using Dictionary.Model.API;
using Dictionary.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Model
{
    public class TranslateAPI
    {
        private static readonly string key = App.Current.Resources["AzureTranslatorKey"].ToString();
        private static readonly string endpoint = App.Current.Resources["AzureTranslatorEndpoint"].ToString();

        public static async Task<ApiResponse<string>> Translate(string text, string? sourceLangCode = "vi", string? translateLangCode = "en", ILogger<BaseViewModel>? logger = null)
        {
            // Input and output languages are defined as parameters.
            string route = $"/translate?api-version=3.0&from={sourceLangCode}&to={translateLangCode}";
            object[] body = new object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                // Send the request and get response.
                try
                {
                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                    //Check if response is OK
                    if ((int)response.StatusCode == 200)
                    {

                        // Read response as a string.
                        string result = await response.Content.ReadAsStringAsync();
                        return new ApiResponse<string>
                        {
                            Data = result,
                            IsSuccess = true
                        };
                    }
                    else
                    {
                        // Handle HTTP errors with specific error codes
                        // Parse the JSON error response

                        logger.LogError($"Translate word failed. Error code: {response.StatusCode}");

                        var errorResponse = JsonConvert.DeserializeObject<ApiResponse<string>>(await response.Content.ReadAsStringAsync());

                        return new ApiResponse<string>
                        {
                            Data = "",
                            IsSuccess = false,
                            Error = errorResponse.Error
                        };
                    }
                }
                catch (Exception e)
                {
                    logger.LogError($"Translate word failed. Exception: " + e.Message);
                }

            }
            logger.LogError($"Translate word failed.");
            return new ApiResponse<string>
            {
                Data = "",
                IsSuccess = false,
                Error = new ErrorDetails
                {
                    Code = 0,
                    Message = "Translate word failed."
                }
            };
        }
    }
}
