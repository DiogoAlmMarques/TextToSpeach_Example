using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.IO;

namespace TextToSpeachAzure
{
    class TextToSpeach
    {
        public SpeechConfig inicializarAPI(string text,string voz,string FileName)
        {

                using (FileStream fs = File.OpenWrite(FileName))
                {

                Uri endpoint = new Uri(ConfigurationManager.AppSettings.Get("API_ENDPOINT"));
                string apikey = ConfigurationManager.AppSettings.Get("API_KEY");


                fs.Close();
                var config = SpeechConfig.FromEndpoint(endpoint: endpoint, subscriptionKey: apikey);

                config.SpeechSynthesisVoiceName = voz;

                using var audioConfig = AudioConfig.FromWavFileOutput(FileName);

                SpeechSynthesizer synthesizer = new SpeechSynthesizer(config, audioConfig);
                var result = synthesizer.SpeakTextAsync(text);

                result.Result.Dispose();
                fs.Close();
                audioConfig.Dispose();

                
                }
                return null;
        
           
        }
    }
}

