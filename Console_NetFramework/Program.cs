using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHome.Models;
using static System.Console;
namespace Console_NetFramework
{
	class Program
	{
		static void Main(string[] args)
		{
			Home home = new HomeBuilder()
				.SetFamilyName("Cohen")
				.AddFamilyMember("Ronen")
				.AddFamilyMember("Efrat")
				.AddFamilyMember("Tahel")
				.AddFamilyMember("Ziv")
				.AddDevice(new Device("CCTV Camera #2"))
				.AddDevice(new Device("Electrice window #1"))
				.AddDevice(new Device("Electrice window #2"))
				.AddDevice(new Device("Electrice window #3"))
				.RetrieveHome();

			WriteLine(home);
		}
	}
}
