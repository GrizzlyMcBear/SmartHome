using System;
using System.Collections.Generic;
using System.Text;
using static System.Environment;
namespace SmartHome.Models
{
	public class Home
	{
		//todo: add validation & verification
		#region Members

		private Guid id;

		//todo: consider changing access modifiers to less than public
		public Guid Id
		{
			get => id;
			set => id = value;
		}

		private string familyName;

		public string FamilyName
		{
			get => familyName;
			set => familyName = value;//todo: consider changing set's access modifier to less than public
		}

		private readonly List<string> familyMembers = new List<string>();

		public List<string> FamilyMembers => familyMembers;

		private readonly List<Device> devices = new List<Device>();

		public List<Device> Devices => devices;

		#endregion Members

		#region Construction

		public Home()
		{
		}

		#endregion Construction

		#region Stringification

		public override string ToString()
		{
			var sb = new StringBuilder()
				.AppendLine($"Home Details")
				//.AppendLine($"Home ID is {id}")
				.AppendLine($"Family name is {familyName}");

			// Add family members
			sb.Append($"Family members are ");
			foreach (var member in familyMembers)
				sb.Append($"{member}, ");

			sb.AppendLine();

			// Add devices
			sb.AppendLine($"Devices in home are");
			foreach (var device in devices)
				sb.Append($"* {device}");

			return sb.ToString();
		}

		#endregion Stringification
	}
}
