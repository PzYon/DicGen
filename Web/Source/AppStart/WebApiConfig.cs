using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoXpres.DicGen.Web.AppStart
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// API route for controllers which have specific methods
			GlobalConfiguration.Configuration.Routes.MapHttpRoute(
				"DefaultApi", "api/{controller}/{action}/{id}",
				new {id = RouteParameter.Optional});

			// route for normal web pages
			RouteTable.Routes.MapRoute(
				"Web", "{controller}/{action}/{id}",
				new {controller = "Web", action = "Index", id = UrlParameter.Optional});
		}
	}
}