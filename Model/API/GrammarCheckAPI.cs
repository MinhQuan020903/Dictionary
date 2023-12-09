using Dictionary.ViewModel;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Windows;
using SerpApi;
using MaterialDesignColors;

namespace Dictionary.Model.API
{
    class GrammarCheckApi
    {
        private static Dictionary<string, string> grammarCheck = new Dictionary<string, string>() { { "vi", "vn" }, { "en", "us" } };

        public static string PostCheckGrammar(string paragraph, string from)
        {
            string apiKey = "03b07a1671fac4beb3bd37cb6e8c75087f9b3d52752d3658a816a949af8c60b4";

            // Localized search for Coffee shop in Austin Texas
            Hashtable ht = new Hashtable();
            ht.Add("location", "Vietnam");
            ht.Add("q", paragraph);
            ht.Add("hl", from);
            ht.Add("gl", grammarCheck[from]);
            ht.Add("google_domain", "google.com");

            try
            {
                GoogleSearch search = new GoogleSearch(ht, apiKey);
                JObject data = search.GetJson();
                var search_information = data["search_information"];
                if (search_information != null)
                {
                    if (search_information != null && search_information["spelling_fix"] != null)
                    {
                        return (string)search_information["spelling_fix"].ToString();
                    }
                }

                return paragraph;
            }
            catch (SerpApiSearchException ex)
            {
                MessageBox.Show(ex.ToString(), "Có lỗi xảy ra");
            }
            return paragraph;
        }
    }
}
