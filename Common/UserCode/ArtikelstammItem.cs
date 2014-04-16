namespace LightSwitchApplication
{
	using System;

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
	};
}
