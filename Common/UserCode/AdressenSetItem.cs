using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class AdressenSetItem
	{
		public override string ToString()
		{
			var items = new string[]
			{
				Anrede,
				Name, 
				zHd_Besteller_optional,
				Straße + ' ' + Hausnummer, 
				PLZ + ' ' + Stadt, 
				Land 
			}.Where(n => !String.IsNullOrWhiteSpace(n)).ToArray();

			string result = String.Join(", ", items);

			return String.IsNullOrWhiteSpace(result) ? base.ToString() : result;
		}
	};
}
