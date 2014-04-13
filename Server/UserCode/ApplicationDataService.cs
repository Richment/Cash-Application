using System;
using System.Collections.Generic;
using System.Linq;

namespace LightSwitchApplication
{
	public partial class ApplicationDataService
	{
		// E-MAIL
		partial void OutgoingMailSet_Inserting(OutgoingMail entity)
		{
			entity.Sended = DateTime.Now;
			SmtpSender.SendEmail(entity);
		}
		partial void OutgoingMailSet_Updating(OutgoingMail entity)
		{
			entity.Sended = DateTime.Now;
			SmtpSender.SendEmail(entity);
		}


		// DOCUMENT
		partial void DocumentsSet_Inserting(Documents entity)
		{
			var desc = DocDescriptor.FromByteArray(entity.Data);
			if (desc != null)
			{
				entity.Bezeichnung = desc.Auftragsnummer + " - " + desc.Titel + " vom " + entity.Datum.ToShortDateString();
				entity.GeneratedDocument.Documents = entity;
				entity.GeneratedDocument.Bytes = DocumentGenerator.DocumentToPdf(desc);
			}
		}
		partial void DocumentsSet_Updating(Documents entity)
		{
			var desc = DocDescriptor.FromByteArray(entity.Data);
			if (desc != null)
			{
				entity.Bezeichnung = desc.Auftragsnummer + " - " + desc.Titel + " vom " + entity.Datum.ToShortDateString();
				entity.GeneratedDocument.Documents = entity;
				entity.GeneratedDocument.Bytes = DocumentGenerator.DocumentToPdf(desc);
			}
		}

		// TEMPLATE
		partial void ReportingTemplatesSet_Inserting(ReportingTemplates entity)
		{
			if ((entity.ReleaseDate == ReportingTemplates.Minimum) && (entity.Template.Length == 0) && String.IsNullOrWhiteSpace(entity.OriginalFilename))
			{
				entity.Beschreibung = "Standardvorlage";
				entity.Template = DocumentGenerator.DefaultTemplate;
			}
		}
	};
}
