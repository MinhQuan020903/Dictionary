using Dictionary.Model.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Model.API
{
    public class DictionaryExampleAPI
    {
        private static readonly string key = App.Current.Resources["AzureTranslatorKey"].ToString();
        private static readonly string endpoint = App.Current.Resources["AzureTranslatorEndpoint"].ToString();

        public static async Task<ApiResponse<DictionaryExample>> GetExample(string text, string translation, string? sourceLangCode = "vi", string? translateLangCode = "en")
        {
            // Input and output languages are defined as parameters.
            string route = $"/dictionary/examples?api-version=3.0&from={sourceLangCode}&to={translateLangCode}";
            object[] body = new object[] { new { Text = text.Normalize(), Translation = translation.Normalize()  }
    };
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
                        List<DictionaryExample> dictionaryExamples = JsonConvert.DeserializeObject<List<DictionaryExample>>(result);

                        if (dictionaryExamples != null)
                        {
                            return new ApiResponse<DictionaryExample>
                            {
                                Data = dictionaryExamples[0],
                                IsSuccess = true
                            };
                        }
                        else
                        {
                            return new ApiResponse<DictionaryExample>
                            {
                                Data = null,
                                IsSuccess = false,
                                Error = new ErrorDetails
                                {
                                    Code = 0,
                                    Message = "Couldn't translate."
                                }
                            };
                        }

                    }
                    else
                    {
                        // Handle HTTP errors with specific error codes
                        // Parse the JSON error response

                        var errorResponse = JsonConvert.DeserializeObject<ApiResponse<string>>(await response.Content.ReadAsStringAsync());

                        return new ApiResponse<DictionaryExample>
                        {
                            Data = null,
                            IsSuccess = false,
                            Error = errorResponse.Error
                        };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return new ApiResponse<DictionaryExample>
            {
                Data = null,
                IsSuccess = false,
                Error = new ErrorDetails
                {
                    Code = 0,
                    Message = "Unknown error"
                }
            };
        }
        private static string RemoveEncoding(string encodedJson)
        {
            var sb = new StringBuilder(encodedJson);
            sb.Replace("\\", string.Empty);
            sb.Replace("\"[", "[");
            sb.Replace("]\"", "]");
            return sb.ToString();
        }
    }
}
