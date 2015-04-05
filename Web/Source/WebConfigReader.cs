using System.Configuration;

namespace DoXpres.DicGen.Web
{
	public static class WebConfigReader
	{
		public static T Get<T>(string key)
		{
			AppSettingsReader x = new AppSettingsReader();
			return (T) x.GetValue(key, typeof (T));
		}
	}
}