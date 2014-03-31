namespace LightSwitchApplication
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	public class DocDescriptor
	{
		#region Constants

		public const string TITLE = "titel";
		public const string ADDRESS = "adresse";
		public const string R_NR = "rechnungsnr";
		public const string R_DATE = "rechnungsdatum";
		public const string A_NR = "auftragsnr";
		public const string L_NR = "lieferscheinnr";
		public const string L_DATE = "lieferdatum";
		public const string L_AMOUNT = "lieferkosten";
		public const string BRUTTO = "brutto";
		public const string NETTO = "gesamtnetto";
		public const string TAX = "mehrwertsteuer";

		#endregion

		public class PositionDescription
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

			private static PositionDescription FromString(string value)
			{
				var parts = value.Split(new string[] { ITEM_SEPERATOR }, StringSplitOptions.None);
				try
				{
					return new PositionDescription()
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

		public static explicit operator Dictionary<string, string>(DocDescriptor value)
		{
			return value.ToDictionary();
		}
		public static explicit operator DocDescriptor(Dictionary<string, string> value)
		{
			return new DocDescriptor(value);
		}

		public string Titel
		{
			get
			{
				string result;
				if (data.TryGetValue(TITLE, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[TITLE] = value;
				else
					data.Remove(TITLE);
			}
		}
		public string Adresse
		{
			get
			{
				string result;
				if (data.TryGetValue(ADDRESS, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[ADDRESS] = value;
				else
					data.Remove(ADDRESS);
			}
		}
		public string Datum
		{
			get
			{
				string result;
				if (data.TryGetValue(R_DATE, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[R_DATE] = value;
				else
					data.Remove(R_DATE);
			}
		}
		public string Rechnungsnummer
		{
			get
			{
				string result;
				if (data.TryGetValue(R_NR, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[R_NR] = value;
				else
					data.Remove(R_NR);
			}
		}
		public string Auftragsnummer
		{
			get
			{
				string result;
				if (data.TryGetValue(A_NR, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[A_NR] = value;
				else
					data.Remove(A_NR);
			}
		}
		public string Lieferscheinnummer
		{
			get
			{
				string result;
				if (data.TryGetValue(L_NR, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[L_NR] = value;
				else
					data.Remove(L_NR);
			}
		}
		public string Lieferdatum
		{
			get
			{
				string result;
				if (data.TryGetValue(L_DATE, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[L_DATE] = value;
				else
					data.Remove(L_DATE);
			}
		}
		public string Lieferkosten
		{
			get
			{
				string result;
				if (data.TryGetValue(L_AMOUNT, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[L_AMOUNT] = value;
				else
					data.Remove(L_AMOUNT);
			}
		}
		public string Brutto
		{
			get
			{
				string result;
				if (data.TryGetValue(BRUTTO, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[BRUTTO] = value;
				else
					data.Remove(BRUTTO);
			}
		}
		public string Netto
		{
			get
			{
				string result;
				if (data.TryGetValue(NETTO, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[NETTO] = value;
				else
					data.Remove(NETTO);
			}
		}
		public string Mehrwertsteuer
		{
			get
			{
				string result;
				if (data.TryGetValue(TAX, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[TAX] = value;
				else
					data.Remove(TAX);
			}
		}

		public PositionCollection Positionen
		{
			get;
			private set;
		}

		private Dictionary<string, string> data;

		public DocDescriptor()
		{
			data = new Dictionary<string, string>();
			Positionen = new PositionCollection();
		}
		public DocDescriptor(Dictionary<string, string> initial)
			: this()
		{
			foreach (var key in initial.Keys.Except(new string[] { String.Empty }))
				data.Add(key, initial[key]);
			string positionString;
			if (initial.TryGetValue(String.Empty, out positionString))
				Positionen = PositionCollection.FromString(positionString);
		}


		public Dictionary<string, string> ToDictionary()
		{
			data[String.Empty] = Positionen.ToString();
			return data.Where(n => !String.IsNullOrWhiteSpace(n.Value)).ToDictionary(n => n.Key, m => m.Value);
		}
	};
}