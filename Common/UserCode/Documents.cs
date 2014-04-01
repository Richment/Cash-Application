﻿namespace LightSwitchApplication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.LightSwitch;

	public partial class Documents
	{
		partial void Documents_Created()
		{
			this.Datum = DateTime.Now;
			this.Bezeichnung = String.Empty;
		}

		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(Bezeichnung) ? base.ToString() : Bezeichnung;
		}
	};
}
