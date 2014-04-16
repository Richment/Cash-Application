using System.Linq;

namespace LightSwitchApplication
{
	public partial class EMailSettings
	{
		partial void EMailSettings_Created()
		{
			if (MailSettingsSet.Count == 0)
			{
				MailSettingsSet.SelectedItem = MailSettingsSet.AddNew();
			}
			else
			{
				MailSettingsSet.SelectedItem = MailSettingsSet.First();
			}
		}
	   
		partial void EMailSettings_Saved()
		{
			this.Close(false);
		}
	}
}