namespace LightSwitchApplication
{
	using System;

	public partial class OutgoingMail
	{
		public override string ToString()
		{
			if (String.IsNullOrWhiteSpace(Subject))
				return base.ToString();
			return Subject;
		}
	};
}
