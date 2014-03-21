﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LightSwitchApplication
{
	public partial class Rechnungen
	{
		private static List<byte[]> images;

		static Rechnungen()
		{
			var names = new string[]
			{
				"grau.jpg",
				"gelb.jpg",
				"orange.jpg",
				"blau_hell.jpg",
				"blau_dunkel.jpg",
				"grün.jpg",
				"rot.jpg"
			};

			images = new List<byte[]>();
			var assembly = Assembly.GetExecutingAssembly();
			for (int i = 0; i < names.Length; i++)
			{
				var resName = "LightSwitchApplication." + names[i];
				using (Stream resourceStream = assembly.GetManifestResourceStream(resName))
				{
					int length = (int)resourceStream.Length;
					byte[] bytes = new byte[length];
					resourceStream.Read(bytes, 0, length);
					images.Add(bytes);
				}
			}
		}

		partial void Rechnungsbetrag_Netto_Compute(ref decimal? result)
		{
			result = this.Netto_Gesamtbetrag + Lieferkosten;
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void Rechnungsbetrag_Brutto_Compute(ref decimal? result)
		{
			if (this.Rechnungsbetrag_Netto.HasValue)
			{
				result = this.Rechnungsbetrag_Netto.Value + (this.Rechnungsbetrag_Netto * 19 / 100);
				if (result.HasValue)
					result = Math.Round(result.Value, 2);
			}
		}

		partial void Mehrwertsteuer_Compute(ref decimal? result)
		{
			result = this.Rechnungsbetrag_Netto * 19 / 100;
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void Adresse_Compute(ref string result)
		{
			string[] elements = new string[]
			{
				Kunde.Firma,
				Kunde.Vorname +' '+ Kunde.Nachnahme,
				Kunde.Straße +' '+ Kunde.Hausnummer,
				Kunde.PLZ +' '+ Kunde.Stadt,
				Kunde.Land
			};
			result = String.Join(Environment.NewLine, elements);
		}

		partial void Netto_Gesamtbetrag_Compute(ref decimal? result)
		{
			result = this.ArtikellisteCollection.Sum(i => i.Preis);
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void StatusImage_Compute(ref byte[] result)
		{
			result = images[this.Status];
		}

		public override string ToString()
		{
			try
			{
				string besteller = Kunde.Vorname + ' ' + Kunde.Nachnahme;
				if (!String.IsNullOrWhiteSpace(Kunde.Firma))
					besteller = Kunde.Firma + ", " + besteller;
				return String.Format("[{0}]\t  {1}  ({2})",this.Bestelldatum.ToString("d"),  Referenznummer , besteller );
			}
			catch
			{
				return base.ToString();
			}
		}

	}
}
