using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.LightSwitch;

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
			result = Netto_Gesamtbetrag + Lieferkosten.GetValueOrDefault(0);
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void Rechnungsbetrag_Brutto_Compute(ref decimal? result)
		{
			if (this.Rechnungsbetrag_Netto.HasValue)
			{
				result = Rechnungsbetrag_Netto.Value + Mehrwertsteuer.Value;
				if (result.HasValue)
					result = Math.Round(result.Value, 2);
			}
		}

		partial void Mehrwertsteuer_Compute(ref decimal? result)
		{
			if (this.Rechnungsbetrag_Netto.HasValue)
			{
				result = Rechnungsbetrag_Netto * 19 / 100;
				if (Mahnung.GetValueOrDefault(false))
					result += Mahnkosten;
				if (result.HasValue)
					result = Math.Round(result.Value, 2);
			}
		}

		partial void Adresse_Compute(ref string result)
		{
			if (Kunde == null)
				return;

			string[] elements = new string[]
			{
				String.IsNullOrWhiteSpace(Kunde.Firma) ? Kunde.Anrede : Kunde.Firma,
				Kunde.Vorname +' '+ Kunde.Nachnahme,
				Kunde.Straße +' '+ Kunde.Hausnummer,
				Kunde.PLZ +' '+ Kunde.Stadt,
				Kunde.Land
			};
			result = String.Join(Environment.NewLine, elements);
		}

		partial void Netto_Gesamtbetrag_Compute(ref decimal? result)
		{
			result = ArtikellisteCollection.Sum(i => i.Preis);
			if (result.HasValue)
				result = Math.Round(result.Value, 2);
		}

		partial void StatusImage_Compute(ref byte[] result)
		{
			result = images[Status];
		}

		partial void Status_Validate(EntityValidationResultsBuilder results)
		{
			if (Status == (int)Bestellstatus.Geliefert)
			{
				if (Kunde == null)
					return;

				if (Rechnungsdatum.HasValue)
				{
					if (Rechnungsdatum.Value.AddDays(Kunde.Zahlungsziel) < DateTime.Today)
					{
						Status = (int)Bestellstatus.Zahlungsverzug;
					}
				}
				else
				{
					results.AddEntityResult("Ohne Rechnungdatum kann kein Zahlungsverzug ermittelt werden.", ValidationSeverity.Informational);
				}
			}
		}

		partial void Bearbeitungsstatus_Compute(ref string result)
		{
			result = RequiresProcessing ? "In Auftragssammlung" : "";
		}

		partial void Mahnkosten_Compute(ref decimal result)
		{
			if (Rechnungsdatum == null)
				return;
			result = 30M + (decimal)Math.Round(Rechnungsbetrag_Netto.Value * ((int)(Bezahldatum ?? DateTime.Today).Subtract(Rechnungsdatum.Value).TotalDays) * 0.085M / 360, 2);
		}

		public override string ToString()
		{
			try
			{
				string result = Kunde.Vorname + ' ' + Kunde.Nachnahme;
				if (!String.IsNullOrWhiteSpace(Kunde.Firma))
					result = Kunde.Firma + ", " + result;
				return String.Format("[{0}]\t  {1}  ({2})", this.Bestelldatum.ToString("d"), Referenznummer, result);
			}
			catch
			{
				return base.ToString();
			}
		}
	};
}
