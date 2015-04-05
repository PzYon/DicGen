using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DoXpres.DicGen.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoXpres.DicGen.LibraryTests
{
	[TestClass]
	public class GeneratorTests
	{
		[TestMethod]
		public void Generate_NullInput_ReturnsEmptyEnumerable()
		{
			GeneratorParams gp = GetGeneratorParams(null);

			Assert.IsNotNull(Generator.Generate(gp));
			Assert.AreEqual(0, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_EmtpyInput_ReturnsEmptyEnumerable()
		{
			GeneratorParams gp = GetGeneratorParams(String.Empty);

			Assert.IsNotNull(Generator.Generate(gp));
			Assert.AreEqual(0, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_WhiteSpaceInput_ReturnsEmptyEnumerable()
		{
			GeneratorParams gp = GetGeneratorParams("  ");

			Assert.IsNotNull(Generator.Generate(gp));
			Assert.AreEqual(0, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_SimpleInput_ReturnsEnumerable()
		{
			GeneratorParams gp = GetGeneratorParams("Roger Federer");

			Assert.AreEqual(2, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_SimpleInput_OneCharacterWordsAreRemoved()
		{
			GeneratorParams gp = GetGeneratorParams("A cow is an animal. I am not.");

			Assert.AreEqual(6, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_InputWithUmlauts_ArentRemoved()
		{
			GeneratorParams gp = GetGeneratorParams("äöü éèêàç äbä öhö.");

			Assert.AreEqual(4, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_ResultsAreLowerCase()
		{
			GeneratorParams gp = GetGeneratorParams("AlPhA");

			Assert.AreEqual("alpha", Generator.Generate(gp).First());
		}

		[TestMethod]
		public void Generate_InputWithQuoetes_ResultsAreLowerCase()
		{
			GeneratorParams gp = GetGeneratorParams("S'Mami");

			Assert.AreEqual("s'mami", Generator.Generate(gp).First());
		}

		[TestMethod]
		public void Generate_AllUpperCase_IsUntouched()
		{
			GeneratorParams gp = GetGeneratorParams("XML");

			Assert.AreEqual("XML", Generator.Generate(gp).First());
		}

		[TestMethod]
		public void Generate_MultipleValues_ResultItemsAreDistinct()
		{
			GeneratorParams gp = GetGeneratorParams("Stan Roger Roger Federer Roger Stan");
			
			Assert.AreEqual(3, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_ValuesWithDifferentCasing_ResultItemsAreDistinct()
		{
			GeneratorParams gp = GetGeneratorParams("Roger roger");

			Assert.AreEqual(1, Generator.Generate(gp).Count());
		}


		[TestMethod]
		public void Generate_AllResultItemsAreTrimmed()
		{
			GeneratorParams gp = GetGeneratorParams("   asd  s df sd f    sd f f   sdfd  sd sd f   sf ff ff   sdf ");

			Assert.IsTrue(Generator.Generate(gp).All(w => w.Length == w.Trim().Length));
		}

		[TestMethod]
		public void Generate_NoDigitsInResultItems()
		{
			GeneratorParams gp = GetGeneratorParams("123 abc1abc 1 12 123 1234 abc1 1abc 1abc1");

			IEnumerable<string> enumerable = Generator.Generate(gp);
			Assert.IsTrue(enumerable.All(w => !Regex.IsMatch(w, "\\d")));
		}

		[TestMethod]
		public void Generate_SortOrderAlphabetically_SortsAlphabetically()
		{
			GeneratorParams gp = GetGeneratorParams("beta gamma alpha", SortOrder.Alphabetically);

			string[] result = Generator.Generate(gp).ToArray();

			Assert.AreEqual("alpha", result[0]);
			Assert.AreEqual("beta", result[1]);
			Assert.AreEqual("gamma", result[2]);
		}

		[TestMethod]
		public void Generate_SortOrderWordLength_SortsByWordLength()
		{
			GeneratorParams gp = GetGeneratorParams("abcde abcd abc", SortOrder.WordLength);

			string[] result = Generator.Generate(gp).ToArray();

			Assert.AreEqual("abc", result[0]);
			Assert.AreEqual("abcd", result[1]);
			Assert.AreEqual("abcde", result[2]);
		}

		[TestMethod]
		public void Generate_InputWithSingleQuote_DoesntRemoveSingleQuote()
		{
			const string input = "goht's";
			const string expectedOutput = input;

			Assert.AreEqual(Generator.Generate(GetGeneratorParams(input)).First(), expectedOutput);
		}

		[TestMethod]
		public void Generate_InputOnMultipleLines_AreTreatedAsWords()
		{
			GeneratorParams gp = GetGeneratorParams("Hallo\nRoger\nFederer");

			Assert.AreEqual(3, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_SpecialChars_AreStripped()
		{
			GeneratorParams gp = GetGeneratorParams("Ach..");

			Assert.AreEqual("ach", Generator.Generate(gp).First(), true);
		}

		[TestMethod]
		public void Generate_Quotes_AreStripped()
		{
			GeneratorParams gp = GetGeneratorParams("\"nice\"");

			Assert.AreEqual("nice", Generator.Generate(gp).First(), true);
		}

		[TestMethod]
		public void Generate_SingleQuotes_AreStripped()
		{
			GeneratorParams gp = GetGeneratorParams("'nice'");

			Assert.AreEqual("nice", Generator.Generate(gp).First(), true);
		}

		[TestMethod]
		public void Generate_ReprocessingInput_DoesntChangeOutput()
		{
			GeneratorParams gp = GetGeneratorParams("Sali zämme, wie goht's? Ich bi de FCb usem Johr 1886, oder so.");

			IEnumerable<string> firstResult = Generator.Generate(gp);
			IEnumerable<string> secondResult = Generator.Generate(GetGeneratorParams(String.Join(" ", firstResult)));

			Assert.AreEqual(String.Join("|", firstResult), String.Join("|", secondResult));
		}

		[TestMethod]
		public void Generate_StreeeeetchedWords_AreRemoved()
		{
			GeneratorParams gp = GetGeneratorParams("Rooooooooooogeeer, Feeederer ist der Beeeeeste.");

			Assert.AreEqual(2, Generator.Generate(gp).Count());
		}

		[TestMethod]
		public void Generate_URLs_AreRemoved()
		{
			GeneratorParams gp = GetGeneratorParams("http://www.doxpres.ch/dicgen");

			Assert.AreEqual(0, Generator.Generate(gp).Count());
		}

		private static GeneratorParams GetGeneratorParams(string text, SortOrder sortOrder = SortOrder.Alphabetically)
		{
			return new GeneratorParams { Text = text, SortOrder = sortOrder};
		}

		private static bool AreEqual(IEnumerable<string> ie1, IEnumerable<string> ie2)
		{
			return String.Join(",", ie1) == String.Join(",", ie2);
		}
	}
}