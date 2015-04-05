using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DoXpres.DicGen.Library
{
	public static class Generator
	{
		private static readonly Regex isRegularWordRegEx = new Regex(@"\b(([a-zäöüéèêàç'])(?!\2{2,})){2,}\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static readonly Regex isOnlyUpperRegEx = new Regex(@"^\p{Lu}+$", RegexOptions.Compiled);

		public static IEnumerable<string> Generate(GeneratorParams generatorParams)
		{
			if (String.IsNullOrEmpty(generatorParams.Text))
			{
				return Enumerable.Empty<string>();
			}

			IEnumerable<string> words = generatorParams.Text
			                                           .Split(' ', '\n')
			                                           .Where(s => !s.StartsWith("http") && !s.Contains("@"))
			                                           .SelectMany(w => isRegularWordRegEx.Matches(w).OfType<Match>())
			                                           .Select(m => m.Groups[0].Value)
			                                           .Select(w => isOnlyUpperRegEx.IsMatch(w) ? w : w.ToLower())
			                                           .Distinct();

			switch (generatorParams.SortOrder)
			{
				case SortOrder.WordLength:
					words = words.OrderBy(w => w.Length);
					break;
				case SortOrder.Alphabetically:
				default:
					words = words.OrderBy(w => w);
					break;
			}

			return words;
		}
	}
}