using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Models
{
	public class Device
	{
		#region Members
		
		private string name;

		public string Name
		{
			get => name;
			set => name = value;
		}

		private Guid id;

		public Guid Id
		{
			get => id;
			set => id = value;
		}

		#endregion Members

		#region Construction
		
		public Device(string name)
		{
			Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
			Id = Guid.NewGuid();
		}

		#endregion Construction

		#region Stringification

		public override string ToString()
		{
			return new StringBuilder()
				.AppendLine("Device details are")
				.AppendLine($"  Device name is {name}")
				//.AppendLine($"  Device guid is {id}")
				.ToString();
		}

		#endregion Stringification
	}
}
