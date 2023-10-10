using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary.Model
{


    public class TextToSpeechAPI
    {
        // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
        static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        static SpeechSynthesizer speechSynthesizer; // Declare a static instance
        static bool isInitialized = false; // Track initialization status

        static async Task InitializeAsync()
        {
            if (!isInitialized)
            {
                var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                //Vietnamese voice: vi-VN-HoaiMyNeural
                //English voice: en-US-JennyNeural
                speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
                speechSynthesizer = new SpeechSynthesizer(speechConfig);

                // Perform any additional initialization here

                isInitialized = true; // Mark as initialized
            }
        }

        static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"Speech synthesized for text: [{text}]");
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
                default:
                    break;
            }
        }

        public static async Task TextToSpeech(string text)
        {
            // Initialize the speech synthesizer if not already initialized
            await InitializeAsync();

            // Use the preloaded speechSynthesizer instance
            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
            OutputSpeechSynthesisResult(speechSynthesisResult, text);
        }
    }
}
