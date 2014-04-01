namespace LightSwitchApplication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Microsoft.LightSwitch;

	public partial class KundengruppenItem
	{
		public override string ToString()
		{
			if(this.Bezeichnung == null)
				return base.ToString();
			return this.Bezeichnung;
		}
	};
}
