using System;
using System.Threading.Tasks;
using Unsplasharp;

namespace Dictionary.Model
{
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
    }
}
