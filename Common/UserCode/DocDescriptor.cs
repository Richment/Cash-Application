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
		public const string REF = "referenznr";

		#endregion

		public static explicit operator Dictionary<string, string>(DocDescriptor value)
		{
			return value.ToDictionary();
		}
		public static explicit operator DocDescriptor(Dictionary<string, string> value)
		{
			return new DocDescriptor(initial: value);
		}

		public string Referenznummer
		{
			get
			{
				string result;
				if (data.TryGetValue(REF, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[REF] = value;
				else
					data.Remove(REF);
			}
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

		public DocDescriptor(string referenznummer = null, string titel = null, Dictionary<string, string> initial = null)
		{
			data = new Dictionary<string, string>();
			Positionen = new PositionCollection();

			if (initial != null)
			{
				foreach (var key in initial.Keys.Except(new string[] { String.Empty }))
					data.Add(key, initial[key]);

				string positionString;
				if (initial.TryGetValue(String.Empty, out positionString))
					Positionen = PositionCollection.FromString(positionString);
			}

			if (referenznummer != null)
				this.Rechnungsnummer = referenznummer;
			if (titel != null)
				this.Titel = titel;
		}

		public Dictionary<string, string> ToDictionary()
		{
			data[String.Empty] = Positionen.ToString();
			return data.Where(n => !String.IsNullOrWhiteSpace(n.Value)).ToDictionary(n => n.Key, m => m.Value);
		}
	};
}