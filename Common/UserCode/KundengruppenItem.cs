namespace LightSwitchApplication
{

	public partial class KundengruppenItem
	{
		public override string ToString()
		{
			if(this.Bezeichnung == null)
				return base.ToString();
			return this.Bezeichnung;
		}
	};
}
