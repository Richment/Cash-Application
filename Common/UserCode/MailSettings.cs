
namespace LightSwitchApplication
{
	public partial class MailSettings
	{
		partial void MailSettings_Created()
		{
			Port = 25;
			SmtpServer = "smtp.domain.com";
			Username = "user@domain.com";
			SenderAddress = "noreply@domain.com";
			Password = "";
		}
	};
}
