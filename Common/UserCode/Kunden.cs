﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class Kunden
	{
		public override string ToString()
		{
			string result = String.IsNullOrWhiteSpace(this.Firma) ? "" : this.Firma + ", ";
			result += this.Nachnahme.Trim() + ' ' + this.Vorname.Trim();
			return String.IsNullOrWhiteSpace(result) ? base.ToString() : result;
		}
	}
}
