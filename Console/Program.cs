using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using SmartHome.Models;
using static System.Console;
namespace Console_NetFramework
{
	class Program
	{
		static void Main(string[] args)
		{
			Home home = new HomeBuilder()
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

			// Print home details
			WriteLine(home);

			// Speak home details
			var synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			synthesizer.Speak(home.ToString());
		}
	}
}
