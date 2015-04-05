using System.Collections.Generic;
using System.Web.Http;
using DoXpres.DicGen.Library;

namespace DoXpres.DicGen.Web.Http.Controllers.Api
{
	public class GeneratorController : ApiController
	{
		[HttpPost]
		public IEnumerable<string> Generate(GeneratorParams generatorParams)
		{
			return Generator.Generate(generatorParams);
		}
	}
}