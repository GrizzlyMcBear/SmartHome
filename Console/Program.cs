using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using SmartHome.Models;
using static System.Console;
using System.Globalization;

namespace Console_NetFramework
{
	class Program
	{
		#region Members
		
		private static SpeechSynthesizer synthesizer = new SpeechSynthesizer();

		private static SpeechRecognitionEngine speechRecognitionEngine = new SpeechRecognitionEngine();

		static bool done = false;
		
		static bool speechOn = true;

		private static Home home;

		#endregion Members

		private static Home CreateHome()
		{
			return new HomeBuilder()
				.SetFamilyName("Doe")
				.AddFamilyMember("John")
				.AddFamilyMember("Jane")
				.AddFamilyMember("Mickey")
				.AddFamilyMember("Mini")
				.AddDevice(new Device("CCTV Camera #2"))
				.AddDevice(new Device("Electric window #1"))
				.AddDevice(new Device("Electric window #2"))
				.AddDevice(new Device("Electric window #3"))
				.RetrieveHome();
		}

		private static void Initialize()
		{
			home = CreateHome();

			synthesizer.SetOutputToDefaultAudioDevice();
		}

		#region TTS

		private static PromptBuilder GetPromptBuilderForTTS(string tts, string cultureInfo)
		{
			var promptBuilder = new PromptBuilder();
			promptBuilder.StartVoice(new CultureInfo(cultureInfo));
			promptBuilder.AppendText(tts);
			promptBuilder.EndVoice();

			return promptBuilder;
		}

		private static void Speak(string tts, string cultureInfo = "en-US")
		{
			synthesizer.Speak(GetPromptBuilderForTTS(tts, cultureInfo));
		}

		private static void SpeakAsync(string tts, string cultureInfo = "en-US")
		{
			synthesizer.SpeakAsync(GetPromptBuilderForTTS(tts, cultureInfo));
		}

		#endregion TTS

		#region TTS Synthesis Testing

		private static void PrintInstalledLanguages()
		{
			foreach (var voice in synthesizer.GetInstalledVoices())
			{
				var info = voice.VoiceInfo;
				WriteLine($"Id: {info.Id} | Name: {info.Name} | Age: { info.Age} | Gender: { info.Gender} | Culture: { info.Culture}");
			}
		}

		private static void SpeakMultiLanguageSentence()
		{
			synthesizer.SetOutputToDefaultAudioDevice();
			var builder = new PromptBuilder();
			builder.StartVoice(new CultureInfo("en-US"));
			builder.AppendText("All we need to do is to keep talking.");
			builder.EndVoice();
			builder.StartVoice(new CultureInfo("ru-RU"));
			builder.AppendText("Всё, что нам нужно сделать, это продолжать говорить");
			builder.EndVoice();

			synthesizer.Speak(builder);
		}

		private static void Testing_AppendTextWithHint()
		{
			var builder = new PromptBuilder();
			builder.AppendTextWithHint("3rd", SayAs.NumberOrdinal);
			builder.AppendBreak();
			builder.AppendTextWithHint("3rd", SayAs.NumberCardinal);
			builder.AppendBreak();
			builder.AppendBookmark("First bookmark");
			builder.AppendBreak();
			builder.AppendTextWithPronunciation("DuBois", "duˈbwɑ");
			builder.AppendBreak();
			
			synthesizer.Speak(builder);
		}

		#endregion TTS Synthesis Testing

		#region Speech Recognition

		private static void InitializeSpeechRecognition()
		{
			speechRecognitionEngine.SetInputToDefaultAudioDevice();
			speechRecognitionEngine.SpeechRecognized += OnSpeechRecognized;

			Choices basicControlCommandChoices = new Choices();
			basicControlCommandChoices.Add("speech on");
			basicControlCommandChoices.Add("speech off");
			basicControlCommandChoices.Add("test");
			basicControlCommandChoices.Add("exit application");
			basicControlCommandChoices.Add("exit app");

			GrammarBuilder basicControlGrammarBuilder = new GrammarBuilder();
			basicControlGrammarBuilder.Append(basicControlCommandChoices);
			Grammar basicControlGrammar = new Grammar(basicControlGrammarBuilder);
			
			Choices digitChoices = new Choices();
			digitChoices.Add("1");
			digitChoices.Add("2");
			digitChoices.Add("3");
			digitChoices.Add("4");
			digitChoices.Add("5");
			digitChoices.Add("6");
			digitChoices.Add("7");
			digitChoices.Add("8");
			digitChoices.Add("9");
			digitChoices.Add("0");

			GrammarBuilder additionGrammarBuilder = new GrammarBuilder();
			additionGrammarBuilder.Append("What is");
			additionGrammarBuilder.Append("How much is");
			additionGrammarBuilder.Append(digitChoices);
			additionGrammarBuilder.Append("plus");
			additionGrammarBuilder.Append(digitChoices);
			Grammar additionGrammar = new Grammar(additionGrammarBuilder);

			speechRecognitionEngine.LoadGrammarAsync(basicControlGrammar);
			speechRecognitionEngine.LoadGrammarAsync(additionGrammar);
			
			speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
		}

		private static void SpeechRecognition()
		{
			try
			{
				InitializeSpeechRecognition();
				while (done == false) {; }
			}
			catch (Exception ex)
			{
				WriteLine($"Exception encountered: {ex.Message}");
			}
		}

		private static void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		{
			string txt = e.Result.Text;
			float confidence = e.Result.Confidence;
			
			WriteLine("\nRecognized: " + txt);

			if (confidence < 0.60) return;
			
			if (txt.IndexOf("speech on") >= 0)
			{
				WriteLine("Speech is now ON");
				speechOn = true;
			}
			if (txt.IndexOf("speech off") >= 0)
			{
				WriteLine("Speech is now OFF");
				speechOn = false;
			}
			
			if (speechOn == false) return;
			
			if (txt.IndexOf("exit app") >= 0 || txt.IndexOf("exit application") >= 0)
			{
				((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
				done = true;
				WriteLine("Exiting application...");
				Speak("Farewell");
			}
			if (txt.IndexOf("test") >= 0)
			{
				WriteLine("Testing");
				Speak("Testing");
			}
			if (txt.IndexOf("What") >= 0 && txt.IndexOf("plus") >= 0)
			{
				string[] words = txt.Split(' ');
				int num1 = int.Parse(words[2]);
				int num2 = int.Parse(words[4]);
				int sum = num1 + num2;
				WriteLine("(Speaking: " + words[2] + " plus " + words[4] + " equals " + sum + ")");
				SpeakAsync(words[2] + " plus " + words[4] + " equals " + sum);
			}
		}

		#endregion Speech Recognition

		static void Main(string[] args)
		{
			Initialize();

			// Print home details
			//WriteLine(home);

			// Speak the home's details
			//synthesizer.SetOutputToDefaultAudioDevice();
			//synthesizer.SpeakAsync(home.ToString());

			//PrintInstalledLanguages();
			//SpeakMultiLanguageSentence();
			//Testing_AppendTextWithHint();

			SpeechRecognition();

			Write("Press `Enter` to continue...");
			ReadLine();
		}
	}
}
