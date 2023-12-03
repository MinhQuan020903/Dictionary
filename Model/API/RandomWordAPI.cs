using Azure;
using Dictionary.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dictionary.Model.API
{
    public class RandomWordAPI
    {
        private const int TOTAL_WORDS = 30;

        public static async Task<List<string>> GetRandomWordsFromStartCharacter(string character, ILogger<BaseViewModel>? logger = null)
        {
            List<string> randomWords = new List<string>();

            using (HttpClient httpClient = new HttpClient())
            {
                // Add URL
                string query = $"/random/noun/{character}/?count={TOTAL_WORDS}";

                try
                {
                    using HttpResponseMessage response = await httpClient.GetAsync("https://random-word-form.repl.co" + query);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and parse the response content
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        randomWords = JsonConvert.DeserializeObject<List<string>>(jsonResponse);
                    }
                    else
                    {
                        // Handle unsuccessful response
                        logger.LogError($"Error when getting random words from start character. Code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    logger.LogError($"Error when getting random words from start character.");
                }
            }

            return randomWords;
        }
    }

}
