using System.Linq;

namespace LightSwitchApplication
{
	public partial class FirmenDaten
	{
		partial void FirmenDaten_Saved()
		{
			this.Close(false);
		}

		partial void FirmenDaten_Created()
		{
			if (FirmendatenListe.Count == 0)
			{
				FirmendatenListe.SelectedItem = FirmendatenListe.AddNew();
			}
			else
			{
				FirmendatenListe.SelectedItem = FirmendatenListe.First();
			}
		}
	}
}