using System;

namespace LightSwitchApplication
{
	public partial class ReportingTemplates
	{
		public static DateTime Minimum
		{
			get
			{
				return minTime;
			}
		}

		private static readonly DateTime minTime = new DateTime(2000, 1, 1);

		partial void Size_Compute(ref int result)
		{
			if (Template != null)
				result = Template.Length;
		}

		public override string ToString()
		{
			string desc = String.IsNullOrWhiteSpace(Beschreibung) ? "<keine Beschreibung>" : Beschreibung;
			return String.Format("{0} - {1}: {2}", ReleaseDate.ToShortDateString(), ReleaseDate.ToShortTimeString(), desc);
		}
	};
}
