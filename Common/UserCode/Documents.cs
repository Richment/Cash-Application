using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class Documents
	{
		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(Bezeichnung) ? base.ToString() : Bezeichnung;
		}
	}
}
