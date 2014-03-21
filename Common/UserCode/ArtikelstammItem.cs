using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.LightSwitch;

namespace LightSwitchApplication
{
	public partial class ArtikelstammItem
	{
		public override string ToString()
		{
			try
			{
				return String.Format("[{0}] {1}", Artikelnummer, Anzahl_PK == 1 ? Artikelbeschreibung : Anzahl_PK.ToString() + " × " + Artikelbeschreibung);
			}
			catch
			{
				return base.ToString();
			}
		}
	  
	}
}
