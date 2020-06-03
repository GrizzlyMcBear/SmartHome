using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Models
{
	public class HomeBuilder
	{
		#region Members
		
		private Home home;

		#endregion Members

		#region Construction
		
		public HomeBuilder()
		{
			home = new Home();
			home.Id = Guid.NewGuid();
		}

		#endregion Construction

		#region Home Building

		public HomeBuilder AddDevice(Device device)
		{
			home.Devices.Add(device);
			return this;
		}

		public HomeBuilder AddFamilyMember(string name)
		{
			home.FamilyMembers.Add(name);
			return this;
		}

		public HomeBuilder SetFamilyName(string familyName)
		{
			home.FamilyName = familyName ?? throw new ArgumentNullException(paramName: nameof(familyName));
			return this;
		}

		public Home RetrieveHome()
		{
			return home;
		}

		#endregion Home Building
	}
}
