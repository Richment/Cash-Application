namespace LightSwitchApplication
{
	using System;
	using System.Globalization;

	public class Position
	{
		internal const string ITEM_SEPERATOR = "{|}";
		internal const string DATASET_SEPERATOR = "{||}";

		public int Anzahl
		{
			get;
			set;
		}
		public string Artikelnummer
		{
			get;
			set;
		}
		public string Bezeichnung
		{
			get;
			set;
		}
		public decimal PosPreis
		{
			get;
			set;
		}
		public decimal Preis
		{
			get;
			set;
		}

		#region Equals

		public static bool operator ==(Position a, Position b)
		{
			if (ReferenceEquals(a, null))
				return (ReferenceEquals(b, null));
			return a.Equals(b);
		}
		public static bool operator !=(Position a, Position b)
		{
			if (ReferenceEquals(a, null))
				return !(ReferenceEquals(b, null));
			return !a.Equals(b);
		}

		public bool Equals(Position other)
		{
			if (ReferenceEquals(other, null))
				return false;
			if (ReferenceEquals(this, other))
				return true;

			return (Anzahl == other.Anzahl) && (Artikelnummer == other.Artikelnummer) && (Bezeichnung == other.Bezeichnung) && (PosPreis == other.PosPreis) && (Preis == other.Preis);
		}

		public override bool Equals(object obj)
		{
			if (obj is Position)
				return Equals((Position)obj);
			return false;
		}

		public bool Equals(Position x, Position y)
		{
			if (Object.Equals(x, null))
				return Object.Equals(y, null);
			return x.Equals(y);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public int GetHashCode(Position obj)
		{
			if (obj == null)
				throw new ArgumentNullException("obj");
			return obj.GetHashCode();
		}

		#endregion

		public override string ToString()
		{
			return String.Join(ITEM_SEPERATOR, new string[] 
			{ 
				Anzahl.ToString(CultureInfo.InvariantCulture.NumberFormat),
				Artikelnummer, 
				Bezeichnung,
				PosPreis.ToString(CultureInfo.InvariantCulture.NumberFormat),
				Preis.ToString(CultureInfo.InvariantCulture.NumberFormat) 
			});
		}

		internal static Position FromString(string value)
		{
			var parts = value.Split(new string[] { ITEM_SEPERATOR }, StringSplitOptions.None);
			try
			{
				return new Position()
				{
					Anzahl = Int32.Parse(parts[0], CultureInfo.InvariantCulture.NumberFormat),
					Artikelnummer = parts[1],
					Bezeichnung = parts[2],
					PosPreis = Decimal.Parse(parts[3], CultureInfo.InvariantCulture.NumberFormat),
					Preis = Decimal.Parse(parts[4], CultureInfo.InvariantCulture.NumberFormat),
				};
			}
			catch (Exception ex)
			{
				throw new Exception("Extraction failed.", ex);
			}
		}
	};
}
