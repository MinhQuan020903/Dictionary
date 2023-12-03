using Dictionary.ViewModel;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Model.API
{
    public class SpeechToTextAPI
    {
        // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
        static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        static string OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult, ILogger<BaseViewModel> logger)
        {
            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    return speechRecognitionResult.Text.Remove(speechRecognitionResult.Text.Length - 1);
                case ResultReason.NoMatch:
                    {
                        logger.LogError($"SPEECH TO TEXT ERRROR. NOMATCH: Speech could not be recognized.");
                        break;
                    }
                case ResultReason.Canceled:
                    {
                        var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                        logger.LogError($"SPEECH TO TEXT ERRROR. CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            logger.LogError($"SPEECH TO TEXT ERRROR. CANCELED: ErrorCode={cancellation.ErrorCode}");
                            logger.LogError($"SPEECH TO TEXT ERRROR. CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                        }
                        break;
                    }

            }
            return "";
        }

        public async static Task<string> SpeechToText(string SourceLang, ILogger<BaseViewModel> logger)
        {
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            if (SourceLang.Equals("vi"))
            {
                speechConfig.SpeechRecognitionLanguage = "vi-VN";
            }
            else if (SourceLang.Equals("en"))
            {
                speechConfig.SpeechRecognitionLanguage = "en-US";
            }


            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            return OutputSpeechRecognitionResult(speechRecognitionResult, logger);
        }
    }
}
