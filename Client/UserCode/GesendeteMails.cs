using System;

namespace LightSwitchApplication
{
	public partial class GesendeteMails
	{
		partial void Resend_CanExecute(ref bool result)
		{
			result = OutgoingMailSet.SelectedItem != null;
		}

		partial void Resend_Execute()
		{
			OutgoingMailSet.SelectedItem.Sended = DateTime.Now;
			Save();
		}

		partial void OutgoingMailSet_SelectionChanged()
		{
			MessageText = OutgoingMailSet.SelectedItem == null ? "" : OutgoingMailSet.SelectedItem.Body;
			Recipient = OutgoingMailSet.SelectedItem == null ? "" : OutgoingMailSet.SelectedItem.Recipient;
		}
	}
}
