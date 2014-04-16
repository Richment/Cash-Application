namespace LightSwitchApplication
{
	using System;

	public partial class Documents
	{
		partial void Documents_Created()
		{
			this.Datum = DateTime.Now;
			this.Bezeichnung = String.Empty;
			this.GeneratedDocument = new GeneratedDocument() { Documents = this };
		}

		public override string ToString()
		{
			return String.IsNullOrWhiteSpace(Bezeichnung) ? base.ToString() : Bezeichnung;
		}
	};
}
