using System;

namespace LightSwitchApplication
{
	public partial class ReportingTemplates
	{
		partial void Size_Compute(ref int result)
		{
			if (Template != null)
				result = Template.Length;
		}

		public override string ToString()
		{
			return String.Format("{0} - {1}: {2}", ReleaseDate.ToShortDateString(), ReleaseDate.ToShortTimeString(), Beschreibung ?? "");
		}
	};
}
