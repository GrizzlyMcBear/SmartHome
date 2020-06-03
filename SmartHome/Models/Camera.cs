using SmartHome.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHome.Models
{
	public class Camera : Device, IInputDevice
	{
		#region Construction
		
		public Camera(string name) : base(name)
		{
		}

		#endregion Construction

		#region Interfaces

		#region IInputDevice
		
		public void StartInput()
		{
			throw new NotImplementedException();
		}

		public void StopInput()
		{
			throw new NotImplementedException();
		}
		
		#endregion IInputDevice

		#endregion Interfaces
	}
}
