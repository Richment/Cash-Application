namespace LightSwitchApplication
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class DocDescriptor
	{
		#region Constants

		public const string TITLE = "titel";
		public const string ADDRESS = "adresse";
		public const string K_NR = "kundennr";
		public const string R_NR = "rechnungsnr";
		public const string R_DATE = "rechnungsdatum";
		public const string A_NR = "auftragsnr";
		public const string L_NR = "lieferscheinnr";
		public const string L_DATE = "lieferdatum";
		public const string L_AMOUNT = "lieferkosten";
		public const string BRUTTO = "brutto";
		public const string NETTO = "gesamtnetto";
		public const string TAX = "mehrwertsteuer";
		public const string V_DATE = "versanddatum";
		public const string MAHN = "mahnkosten";

		#endregion

		public static explicit operator Dictionary<string, string>(DocDescriptor value)
		{
			return value.ToDictionary();
		}
		public static explicit operator DocDescriptor(Dictionary<string, string> value)
		{
			return new DocDescriptor(initial: value);
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
		public string Kundennummer
		{
			get
			{
				string result;
				if (data.TryGetValue(K_NR, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[K_NR] = value;
				else
					data.Remove(K_NR);
			}
		}
		public string Rechnungsdatum
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
		public string Versanddatum
		{
			get
			{
				string result;
				if (data.TryGetValue(V_DATE, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[V_DATE] = value;
				else
					data.Remove(V_DATE);
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
		public string Mahnkosten
		{
			get
			{
				string result;
				if (data.TryGetValue(MAHN, out result))
					return result;
				return null;
			}
			set
			{
				if (value != null)
					data[MAHN] = value;
				else
					data.Remove(MAHN);
			}
		}
		public PositionCollection Positionen
		{
			get;
			private set;
		}

		private Dictionary<string, string> data;

		public DocDescriptor(string auftragsnummer = null, string titel = null, Dictionary<string, string> initial = null)
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

			if (auftragsnummer != null)
				this.Auftragsnummer = auftragsnummer;
			if (titel != null)
				this.Titel = titel;
		}

		public Dictionary<string, string> ToDictionary()
		{
			data[String.Empty] = Positionen.ToString();
			return data.Where(n => !String.IsNullOrWhiteSpace(n.Value)).ToDictionary(n => n.Key, m => m.Value);
		}

		public byte[] ToByteArray()
		{
			return ToDictionary().Serialize();
		}

		public static DocDescriptor CreateLieferschein(Rechnungen value)
		{
			DocDescriptor result = new DocDescriptor(value.Auftragsnummer, "Lieferschein");

			result.Adresse = value.Lieferadresse == null ? value.Adresse : value.Lieferadresse.ToString();
			if (!String.IsNullOrWhiteSpace(value.Kunde.Kundennummer))
				result.Kundennummer = value.Kunde.Kundennummer;
			if (value.Rechnungsbetrag_Brutto.HasValue)
				result.Brutto = value.Rechnungsbetrag_Brutto.Value.ToString("C");
			if (value.Versanddatum.HasValue)
				result.Versanddatum = value.Versanddatum.Value.ToShortDateString();
			if (value.Lieferkosten.HasValue)
				result.Lieferkosten = value.Lieferkosten.Value.ToString("C");
			if (!String.IsNullOrWhiteSpace(value.Lieferscheinnummer))
				result.Lieferscheinnummer = value.Lieferscheinnummer;
			if (value.Mehrwertsteuer.HasValue)
				result.Mehrwertsteuer = value.Mehrwertsteuer.Value.ToString("C");
			if (value.Netto_Gesamtbetrag.HasValue)
				result.Netto = value.Netto_Gesamtbetrag.Value.ToString("C");

			foreach (var art in value.ArtikellisteCollection)
				result.Positionen.Add((Position)art);

			return result;
		}

		public static DocDescriptor CreateRechnung(Rechnungen value)
		{
			DocDescriptor result = new DocDescriptor(value.Referenznummer, "Rechnung");

			result.Adresse = value.Adresse;
			if (!String.IsNullOrWhiteSpace(value.Rechnungsnummer))
				result.Rechnungsnummer = value.Rechnungsnummer;
			if (value.Rechnungsdatum.HasValue)
				result.Rechnungsdatum = value.Rechnungsdatum.Value.ToShortDateString();
			if (value.Versanddatum.HasValue)
				result.Versanddatum = value.Versanddatum.Value.ToShortDateString();
			if (value.Lieferkosten.HasValue)
				result.Lieferkosten = value.Lieferkosten.Value.ToString("C");
			if (!String.IsNullOrWhiteSpace(value.Lieferscheinnummer))
				result.Lieferscheinnummer = value.Lieferscheinnummer;

			if (value.Rechnungsbetrag_Brutto.HasValue)
				result.Brutto = value.Rechnungsbetrag_Brutto.Value.ToString("C");
			if (value.Mehrwertsteuer.HasValue)
				result.Mehrwertsteuer = value.Mehrwertsteuer.Value.ToString("C");
			if (value.Netto_Gesamtbetrag.HasValue)
				result.Netto = value.Netto_Gesamtbetrag.Value.ToString("C");

			foreach (var art in value.ArtikellisteCollection)
				result.Positionen.Add((Position)art);

			return result;
		}
	};
}