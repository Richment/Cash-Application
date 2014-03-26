using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class OutgoingMail
	{
		public override string ToString()
		{
			if (String.IsNullOrWhiteSpace(Subject))
				return base.ToString();
			return Subject;
		}
	}
}
