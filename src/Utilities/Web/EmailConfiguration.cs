using System.Configuration;
using JosephGuadagno.Utilities.Extensions;

namespace JosephGuadagno.Utilities.Web
{
	public class EmailConfiguration
	{
		public string DisplayName { get; set; }
		public string Host { get; set; }
		public int? Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }

		/// <summary>
		/// Loads the configuration for the email from the application settings
		/// </summary>
		/// <returns>An <see cref="EmailConfiguration"/></returns>
		/// <remarks>
		/// DisplayName = EmailConfiguration.Smtp.DisplayName
		/// Host = EmailConfiguration.Smtp.Host
		/// Port = EmailConfiguration.Smtp.Port
		///	UserName = EmailConfiguration.Smtp.UserName
		///	Password = EmailConfiguration.Smtp.Password
		/// </remarks>
		public static EmailConfiguration GetSettingFromConfigurationFile()
		{
			return new EmailConfiguration
			{
				DisplayName = ConfigurationManager.AppSettings["EmailConfiguration.Smtp.DisplayName"],
				Host = ConfigurationManager.AppSettings["EmailConfiguration.Smtp.Host"],
				Port = ConfigurationManager.AppSettings["EmailConfiguration.Smtp.Port"].To<int?>(),
				UserName = ConfigurationManager.AppSettings["EmailConfiguration.Smtp.UserName"],
				Password = ConfigurationManager.AppSettings["EmailConfiguration.Smtp.Password"]
			};
		}
	}
}