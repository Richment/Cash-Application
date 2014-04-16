
namespace LightSwitchApplication
{
	public partial class KundenÜbersicht
	{
		partial void KundenSetEditSelected_CanExecute(ref bool result)
		{
			result = KundenSet.SelectedItem != null;
		}

		partial void KundenSetEditSelected_Execute()
		{
			Application.ShowKundenDetails(KundenSet.SelectedItem.Id);
		}
	}
}
