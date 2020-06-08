using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string en = "en-US";

		private const string en_regular = "en-US";

		private const string en_question = "en-US";

		private const string ru = "ru-RU";

		private readonly IDictionary<string, string> _messagesByCulture = new Dictionary<string, string>();

		public MainWindow()
		{
			InitializeComponent();
			PopulateMessages();
		}

		private void PromptInEnglish(object sender, RoutedEventArgs e)
		{
			DoPrompt(en);
		}

		private void PromptQuestionInEnglish(object sender, RoutedEventArgs e)
		{
			DoPrompt(en_question);
		}

		private void PromptRegularQuestionInEnglish(object sender, RoutedEventArgs e)
		{
			DoPrompt(en_regular);
		}

		private void PromptInRussian(object sender, RoutedEventArgs e)
		{
			DoPrompt(ru);
		}

		private void DoPrompt(string culture)
		{
			var synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			var builder = new PromptBuilder();
			builder.StartVoice(new CultureInfo(culture));
			builder.AppendText(_messagesByCulture[culture]);
			builder.EndVoice();
			//synthesizer.Speak(builder);
			synthesizer.SpeakAsync(builder);
		}

		private void PopulateMessages()
		{
			_messagesByCulture[en] = "For the connection flight 123 to Saint Petersburg, please, proceed to gate A1";
			_messagesByCulture[en_regular] = "Hello";
			_messagesByCulture[en_question] = "Hello?";
			_messagesByCulture[ru] = "Для пересадки на рейс 123 в  Санкт-Петербург, пожалуйста, пройдите к выходу A1";
		}
	}
}
