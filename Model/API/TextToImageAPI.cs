using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unsplasharp;

namespace Dictionary.Model
{
    public class HttpResponse
    {
        public string? status_url { get; set; } = "";
        public string process_id { get; set; }
        public string status { get; set; }
        public Result result { get; set; }
    }
    public class Result
    {
        public List<string> output { get; set; }
    }
    public class TextToImageAPI
    {
        public static async Task<string> GetImageFromText(string text)
        {
            string unsplashApiKey = App.Current.Resources["UnsplashApiKey"].ToString();
            //Connect to Unsplash API with API credential
            var client = new UnsplasharpClient(unsplashApiKey);
            try
            {
                //Search images based from text
                var photosFound = await client.SearchPhotos(text, 1, 1);
                //Get first image
                string url = photosFound[0].Urls.Small;
                return url;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";
        }
        public static async Task<string> GetImageFromText(string text, string translateLangCode)
        {
            try
            {
                string language = translateLangCode.Equals("vi") ? "vi_VN" : "en_US";
                string prompt = translateLangCode.Equals("vi") ? "Hình ảnh về " + text : "Image about " + text;
                string route = App.Current.Resources["StableDiffusionImageApi"].ToString();
                string key = App.Current.Resources["StableDiffusionImageApiKey"].ToString();

                // Create RestClient and RestRequest
                var client = new RestClient(route);
                var request = new RestRequest("", Method.Post);

                // Add headers
                request.AddHeader("accept", "application/json");
                request.AddHeader("authorization", $"Bearer {key}");
                request.AddHeader("content-type", "application/json");

                // Construct the request body
                var requestBody = new
                {
                    aspect_ratio = "landscape",
                    guidance_scale = 7.5,
                    negprompt = "a blank canvas",
                    prompt = prompt,
                    samples = 1,
                    seed = 2414,
                    steps = 50,
                    style = "anime"
                };

                // Add JSON body to the request
                request.AddJsonBody(requestBody);

                // Execute the request
                var response = await client.ExecuteAsync(request);

                // Check if the response is successful
                if (response.IsSuccessful)
                {
                    // Parse the response content
                    string result = response.Content;
                    string processId = JsonConvert.DeserializeObject<HttpResponse>(result).process_id;
                    string statusUrl = JsonConvert.DeserializeObject<HttpResponse>(result).status_url;

                    // Get the image from the process id 
                    //Through another RestRequest
                    client = new RestClient(statusUrl);
                    request = new RestRequest("", Method.Get);


                    // Add headers
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("authorization", $"Bearer {key}");
                    request.AddHeader("content-type", "application/json");

                    // Execute the request
                    var response2 = await client.ExecuteAsync(request);

                    if (response2.IsSuccessful)
                    {
                        while (!JsonConvert.DeserializeObject<HttpResponse>(response2.Content).status.Equals("COMPLETED"))
                        {
                            response2 = await client.ExecuteAsync(request);
                        }
                        result = response2.Content;
                        var imageUrlList = JsonConvert.DeserializeObject<HttpResponse>(result).result.output;

                        // Check if the list is not empty before accessing the first element
                        string imageUrl = imageUrlList?.FirstOrDefault();
                        return imageUrl;
                    }

                    // You may want to deserialize the result if needed
                    // var responseObject = JsonConvert.DeserializeObject<YourResponseType>(result);

                    // Access the desired information from the response
                    // Example: return responseObject.YourProperty;
                    return "https://random.imagecdn.app/200/150";
                }
                else
                {
                    // Handle error cases
                    MessageBox.Show("Couldn't get image from text. Status code: " + response.StatusCode);
                    return "https://random.imagecdn.app/200/150";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("Couldn't get image from text");
                return "";
            }
        }


    }
}
