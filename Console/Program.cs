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

		private static CultureInfo currentCulture;

		private static SpeechSynthesizer synthesizer;

		private static SpeechRecognitionEngine speechRecognitionEngine;

		static bool done = false;
		
		static bool speechOn = true;

		private static Home home;

		private static string[] systemOptions = { "speech on", "speech off", "shut down" };

		private static string[] userActions = { "hello", "clear text", "add device" };

		private static string[] operators = { "plus", "minus", "times", "divided by" };
		
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
			try
			{
				home = CreateHome();

				currentCulture = new CultureInfo("en-us");

				synthesizer = new SpeechSynthesizer();
				synthesizer.SetOutputToDefaultAudioDevice();

				speechRecognitionEngine = new SpeechRecognitionEngine(currentCulture);
				speechRecognitionEngine.SetInputToDefaultAudioDevice();
			}
			catch (Exception exception)
			{
				WriteLine($"Exception encountered: {exception.Message}");
			}
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

		private static Grammar BuildSystemOptionsGrammar()
		{
			Choices systemOptionsChoices = new Choices();
			foreach (var option in systemOptions)
				systemOptionsChoices.Add(option);

			GrammarBuilder systemOptionsGrammarBuilder = new GrammarBuilder();
			systemOptionsGrammarBuilder.Append(systemOptionsChoices);

			return new Grammar(systemOptionsGrammarBuilder);
		}

		private static Grammar BuildUserActionsGrammar()
		{
			Choices userActionsChoices = new Choices();
			foreach (var action in userActions)
				userActionsChoices.Add(action);

			GrammarBuilder userActionsGrammarBuilder = new GrammarBuilder();
			userActionsGrammarBuilder.Append(userActionsChoices);

			return new Grammar(userActionsGrammarBuilder);
		}

		private static Grammar BuildOperandsAdditionGrammar()
		{
			List<string> numbers = new List<string>();
			for (int currOperand = 0; currOperand < 100; currOperand++)
				numbers.Add(currOperand.ToString());
			
			Choices operandsChoices = new Choices(numbers.ToArray());
			Choices operatorsChoices = new Choices(operators);

			GrammarBuilder additionGrammarBuilder = new GrammarBuilder();
			additionGrammarBuilder.Append("calculate");
			additionGrammarBuilder.Append(operandsChoices);
			additionGrammarBuilder.Append(operatorsChoices);
			additionGrammarBuilder.Append(operandsChoices);

			return new Grammar(additionGrammarBuilder);
		}

		private static void InitializeSpeechRecognition()
		{
			speechRecognitionEngine.SpeechRecognized += OnSpeechRecognized;
			//speechRecognitionEngine.SpeechDetected//todo: research the `SpeechDetected` event

			speechRecognitionEngine.LoadGrammarAsync(BuildSystemOptionsGrammar());
			speechRecognitionEngine.LoadGrammarAsync(BuildOperandsAdditionGrammar());
			speechRecognitionEngine.LoadGrammarAsync(BuildUserActionsGrammar());
			
			speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
		}

		private static void SpeechRecognition()
		{
			try
			{
				SpeakAsync("Awaiting commands");
				while (done == false) {; }
			}
			catch (Exception ex)
			{
				WriteLine($"Exception encountered: {ex.Message}");
			}
		}

		private static void OnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		{
			string text = e.Result.Text;
			float confidence = e.Result.Confidence;
			
			if (confidence < 0.60) return;
			
			if (text.IndexOf("speech on") >= 0)
			{
				WriteLine("Speech is now ON");
				speechOn = true;
			}
			if (text.IndexOf("speech off") >= 0)
			{
				WriteLine("Speech is now OFF");
				speechOn = false;
			}
			
			if (speechOn == false) return;
			
			if (text.IndexOf("shut down") >= 0)
			{
				((SpeechRecognitionEngine)sender).RecognizeAsyncCancel();
				done = true;
				WriteLine("Shutting down...");
				Speak("Bye Bye");
				return;
			}

			var userMessage = "Unidentified command given";

			if (text.IndexOf("clear text") >= 0)
			{
				Clear();
				return;
			}
			else if (text.IndexOf("hello") >= 0)
			{
				userMessage = "Hello to you, how are you today?";
			}
			else if (text.IndexOf("calculate") >= 0)
			{
				string[] words = text.Split(' ');
				
				int num1 = int.Parse(words[1]);
				int num2 = int.Parse(words[words.Length - 1]);
				var operation = words.
					Where((word, index) => index > 1 && index < words.Length - 1)
					.Aggregate(string.Empty, (calculation, word) => calculation += word);
				var result = DoOperation(num1, operation, num2);

				userMessage = $"{num1} {operation} {num2} equals {result}";
			}
			else if (text.IndexOf("add device") >= 0)
			{
				userMessage = "Adding a new device";
				// add a new device
			}

			SpeakAsync(userMessage);
			WriteLine(userMessage);
		}

		private static double DoOperation(int num1, string operation, int num2)
		{
			switch (operation)
			{
				case "plus":
					return num1 + num2;
				case "minus":
					return num1 - num2;
				case "times":
					return num1 * num2;
				case "divided by":
					return num1 / num2;
				default:
					return 0;
			}
		}

		#endregion Speech Recognition

		static void Main(string[] args)
		{
			Initialize();
			InitializeSpeechRecognition();

			// Print home details
			//WriteLine(home);

			// Speak the home's details
			//synthesizer.SetOutputToDefaultAudioDevice();
			//synthesizer.SpeakAsync(home.ToString());

			//PrintInstalledLanguages();
			//SpeakMultiLanguageSentence();
			//Testing_AppendTextWithHint();

			SpeechRecognition();
		}
	}
}
