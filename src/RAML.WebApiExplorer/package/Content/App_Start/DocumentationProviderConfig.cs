using System;
using System.Reflection;
using System.Web;
using System.Web.Http.Description;
using System.Web.Http;

namespace RAML.WebApiExplorer
{
	public static class DocumentationProviderConfig
	{
		public static void IncludeXmlComments(string xmlCommentsPath = null)
		{
			var config = GlobalConfiguration.Configuration;
			if (string.IsNullOrWhiteSpace(xmlCommentsPath))
				xmlCommentsPath = GetXmlCommentsPath();

			config.Services.Replace(typeof (IDocumentationProvider),
			new XmlCommentDocumentationProvider(xmlCommentsPath));
		}

		public static string GetXmlCommentsPath()
		{
			return String.Format(@"{0}\bin\{1}.xml", HttpContext.Current.Server.MapPath(""), Assembly.GetCallingAssembly().GetName().Name);
		} 
	}
}