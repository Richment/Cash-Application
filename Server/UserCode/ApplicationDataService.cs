using System;
using System.Collections.Generic;

namespace LightSwitchApplication
{
	public partial class ApplicationDataService
	{
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

		partial void DocumentsSet_Inserting(Documents entity)
		{
			var data = new Dictionary<string, string>();
			if (data.Deserialize(entity.Data))
			{
				DocDescriptor desc = new DocDescriptor(initial: data);
				entity.Bezeichnung = desc.Auftragsnummer + " - " + desc.Titel + " vom " + entity.Datum.ToShortDateString();
				byte[] wordDoc = DocumentGenerator.ProcessDocument(desc);
				byte[] pdfDoc = DocumentGenerator.DocumentToPdf(wordDoc);
				entity.GeneratedDocument.Documents = entity;
				entity.GeneratedDocument.Bytes = pdfDoc;
			}
		}

		partial void DocumentsSet_Updating(Documents entity)
		{
		   var data = new Dictionary<string, string>();
		   if (data.Deserialize(entity.Data))
		   {
			   DocDescriptor desc = new DocDescriptor(initial: data);
			   entity.Bezeichnung = desc.Auftragsnummer + " - " + desc.Titel + " vom " + entity.Datum.ToShortDateString();
			   byte[] wordDoc = DocumentGenerator.ProcessDocument(desc);
			   byte[] pdfDoc = DocumentGenerator.DocumentToPdf(wordDoc);
			   entity.GeneratedDocument.Documents = entity;
			   entity.GeneratedDocument.Bytes = pdfDoc;
		   }
		}
	}
}
