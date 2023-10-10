using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Model
{
    public class TranslateAPI
    {
        /*public static async Task<string> TranslateText(string text)
        {
            string googleApiKey = App.Current.Resources["GoogleApiKey"].ToString();
            string googleApiUrl = App.Current.Resources["GoogleApiUrl"].ToString();
            //Connect to Google Translate API with API credential
            var client = new Google.Cloud.Translation.V2.TranslationClientBuilder
            {
                JsonCredentials = googleApiKey
            }.Build();
            try
            {
                //Translate text to English
                var response = await client.TranslateTextAsync(text, "en");
                //Get translated text
                string translatedText = response.TranslatedText;
                return translatedText;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }*/
    }
}
