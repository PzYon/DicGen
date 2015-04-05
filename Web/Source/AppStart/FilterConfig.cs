using System.Web.Mvc;

namespace DoXpres.DicGen.Web.AppStart
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}