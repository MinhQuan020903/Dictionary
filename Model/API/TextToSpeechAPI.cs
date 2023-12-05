using Dictionary.ViewModel;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dictionary.Model
{
    public class TextToSpeechAPI
    {
        // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
        private string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        private string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        private SpeechSynthesizer speechSynthesizer; // Declare a static instance
        private bool isInitialized = false; // Track initialization status

        private async Task InitializeAsync(string from, string to)
        {
            if (!isInitialized)
            {
                var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                //Vietnamese voice: vi-VN-HoaiMyNeural
                //English voice: en-US-JennyNeural
                if (from.Equals("vi") && to.Equals("en"))
                {
                    speechConfig.SpeechSynthesisLanguage = "en-US";
                    speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
                }
                else if (from.Equals("en") && to.Equals("vi"))
                {
                    speechConfig.SpeechSynthesisLanguage = "vi-VN";
                    speechConfig.SpeechSynthesisVoiceName = "vi-VN-HoaiMyNeural";
                }

                speechSynthesizer = new SpeechSynthesizer(speechConfig);

                // Perform any additional initialization here

                isInitialized = true; // Mark as initialized
                return;
            }
        }
        private void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text, ILogger<BaseViewModel> logger)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    logger.LogError($"Speech synthesis canceled. Reason: {cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        logger.LogError($"Speech synthesis CANCELED: ErrorCode={cancellation.ErrorCode}");
                        logger.LogError($"Speech synthesis CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        logger.LogError($"Speech synthesis CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
                default:
                    break;
            }
        }

        public  async Task TextToSpeech(string text, string from, string to, ILogger<BaseViewModel>? logger = null)
        {
            // Initialize the speech synthesizer if not already initialized
            await InitializeAsync(from, to);

            // Use the preloaded speechSynthesizer instance
            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
            OutputSpeechSynthesisResult(speechSynthesisResult, text, logger);
        }
    }
}
