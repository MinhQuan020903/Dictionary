﻿using Dictionary.ViewModel;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Dictionary.Model
{


    public class TextToSpeechAPI
    {
        // This example requires environment variables named "SPEECH_KEY" and "SPEECH_REGION"
        static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        static SpeechSynthesizer speechSynthesizer; // Declare a static instance
        static bool isInitialized = false; // Track initialization status

        static async Task InitializeAsync(string from, string to)
        {
            if (!isInitialized)
            {
                var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                //Vietnamese voice: vi-VN-HoaiMyNeural
                //English voice: en-US-JennyNeural
                if (from.Equals("vi") && to.Equals("en"))
                {
                    speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
                }
                else if (from.Equals("en") && to.Equals("vi"))
                {
                    speechConfig.SpeechSynthesisVoiceName = "vi-VN-HoaiMyNeural";
                }

                speechSynthesizer = new SpeechSynthesizer(speechConfig);

                // Perform any additional initialization here

                isInitialized = true; // Mark as initialized
            }
        }

        static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text, ILogger<BaseViewModel> logger)
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

        public static async Task TextToSpeech(string text, string from, string to, ILogger<BaseViewModel>? logger = null)
        {
            // Initialize the speech synthesizer if not already initialized
            await InitializeAsync(from, to);

            // Use the preloaded speechSynthesizer instance
            var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
            OutputSpeechSynthesisResult(speechSynthesisResult, text, logger);
        }
    }
}
