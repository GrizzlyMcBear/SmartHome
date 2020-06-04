using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using SmartHome.Models;
using static System.Console;
using System.Globalization;

namespace Console_NetFramework
{
	class Program
	{
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

		private static void PrintInstalledLanguages(SpeechSynthesizer synthesizer)
		{
			foreach (var voice in synthesizer.GetInstalledVoices())
			{
				var info = voice.VoiceInfo;
				WriteLine($"Id: {info.Id} | Name: {info.Name} | Age: { info.Age} | Gender: { info.Gender} | Culture: { info.Culture}");
			}
		}

		private static void SpeakMultiLanguageSentence(SpeechSynthesizer synthesizer)
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

		private static void Testing_AppendTextWithHint(SpeechSynthesizer synthesizer)
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

		static void Main(string[] args)
		{
			Home home = CreateHome();

			// Print home details
			//WriteLine(home);

			var synthesizer = new SpeechSynthesizer();

			// Speak the home's details
			//synthesizer.SetOutputToDefaultAudioDevice();
			//synthesizer.SpeakAsync(home.ToString());

			//PrintInstalledLanguages(synthesizer);
			//SpeakMultiLanguageSentence(synthesizer);
			//Testing_AppendTextWithHint(synthesizer);

			Write("Press `Enter` to continue...");
			ReadLine();
		}
	}
}
