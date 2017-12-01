namespace Constellation.Foundation.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Globalization;
	using System.Text;

	/// <summary>
	/// String object extensions used to convert Sitecore Item and Field names to C# compatible names.
	/// </summary>
	/// <remarks>
	/// This extension library was forked from Hedgehog Development's T4 starter kit. 
	/// https://github.com/HedgehogDevelopment/tds-codegen.git
	/// I changed the namespace to ensure it doesn't conflict with other TDS projects
	/// you may have running.; I have made some slight modifications to produce class 
	/// names that I find a bit more pleasing.
	/// </remarks>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public static class StringExtensions
	{
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
		public static string TitleCase(this string word)
		{
			var newWord = System.Text.RegularExpressions.Regex.Replace(word, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1+");
			newWord = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(newWord);
			newWord = newWord.Replace("+", string.Empty);
			return newWord;
		}

		public static string CamelCase(this string word)
		{
			if (word == null)
			{
				return string.Empty;
			}

			/*
			 * Something -> something
			 * ISomething -> iSomething
			 * SomethingElse -> somethingElse
			 * ISomethingElse -> iSomethingElse
			 */

			var titleCase = word.TitleCase();
			return titleCase.Substring(0, 1).ToLower() + titleCase.Substring(1);
		}

		public static bool IsInterfaceWord(this string word)
		{
			// looks like an interface if... I[A-Z]xxxx
			// proper definition is http://msdn.microsoft.com/en-us/library/8bc1fexb(v=VS.71).aspx
			return word.Length > 2 && !word.Contains(" ") && (word[0] == 'I' && char.IsUpper(word, 1) && char.IsLower(word, 2));
		}

		public static string AsInterfaceName(this string word)
		{
			/*
			 * return I[TitleCaseWord]
			 * something -> ISomething
			 * Something -> ISomething
			 * ISomething -> ISomething
			 */

			var interfaceWord = GetFormattedWord(word, TitleCase);

			// Only prefix the word with a 'I' if we don't have a word that already looks like an interface.
			if (!word.IsInterfaceWord())
			{
				interfaceWord = string.Concat("I", interfaceWord);
			}

			return interfaceWord;
		}

		public static string AsClassName(this string word)
		{
			// TitleCase the word
			return GetFormattedWord(word, TitleCase);
		}

		public static string AsPropertyName(this string word)
		{
			return GetFormattedWord(word, TitleCase);
		}

		public static string AsFieldName(this string word)
		{
			// return _someParam. 
			// Note, this isn't MS guideline, but it easier to deal with than using this. everywhere to avoid name collisions
			// ReSharper disable RedundantLambdaParameterType
			return GetFormattedWord(word, CamelCase, (string s) => "_" + s);
			// ReSharper restore RedundantLambdaParameterType
		}

		/// <summary>
		/// Tests whether the words conflicts with reserved or language keywords, and if so, attempts to return 
		/// valid words that do not conflict. Usually the returned words are only slightly modified to differentiate 
		/// the identifier from the keyword; for example, the word might be preceded by the underscore ("_") character.
		/// </summary>
		/// <param name="words">The words.</param>
		/// <returns>The list of words with each word cleaned up into a valid word.</returns>
		public static IEnumerable<string> AsValidWords(this IEnumerable<string> words)
		{
			foreach (var word in words)
			{
				yield return AsValidWord(word);
			}
		}

		/// <summary>
		/// Tests whether the word conflicts with reserved or language keywords, and if so, attempts to return a 
		/// valid word that does not conflict. Usually the returned word is only slightly modified to differentiate 
		/// the identifier from the keyword; for example, the word might be preceded by the underscore ("_") character.
		/// <para>
		/// Valid identifiers in C# are defined in the C# Language Specification, item 2.4.2. The rules are very simple:
		/// - An identifier must start with a letter or an underscore
		/// - After the first character, it may contain numbers, letters, connectors, etc
		/// - If the identifier is a keyword, it must be prepended with “@”
		/// </para>
		/// </summary>
		/// <param name="word">The word.</param>
		/// <returns>A valid word for the specified word.</returns>
		public static string AsValidWord(this string word)
		{
			var identifier = word;

			if (identifier == "*")
			{
				identifier = "Wildcard";
			}

			identifier = RemoveDiacritics(identifier);

			// C# Identifiers - http://msdn.microsoft.com/en-us/library/aa664670(VS.71).aspx
			// removes all illegal characters
			var regex = new System.Text.RegularExpressions.Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
			identifier = regex.Replace(identifier, string.Empty);

			// The identifier must start with a character or '_'
			if (!(char.IsLetter(identifier, 0) || identifier[0] == '_'))
			{
				identifier = string.Concat("_", identifier);
			}

			// fix language specific reserved words
			identifier = FixReservedWord(identifier);

			// Let's make sure we have a valid name
			Debug.Assert(System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(identifier), string.Format("'{0}' is an invalid name for a Template or Field", word));
			return identifier;
		}

		/// <summary>
		/// Concatenates all of the <paramref name="words"/> with a '.' separator.
		/// <para>Each word is passed through the <c>AsValidWord</c> method ensuring that it is a valid for a namespace segment.</para>
		/// <para>Leading, trailing, and more than one consecutive '.' are removed.</para>
		/// </summary>
		/// <example> 
		/// This sample shows how to call the <see cref="AsNamespace"/> method.
		/// <code>
		///     string[] segments = new string[5]{ ".My", "Namespace.", "For", "The...Sample..", "Project."};
		///     string ns = segments.AsNamespace();
		/// </code>
		/// The <c>ns</c> variable would contain "<c>My.Namespace.For.The.Sample.Project</c>".
		/// </example>
		/// <param name="words">The namespace segments.</param>
		/// <returns>A valid string in valid namespace format.</returns>
		public static string AsNamespace(this IEnumerable<string> words)
		{
			var joinedNamespace = new List<string>();
			foreach (var segment in words)
			{
				if (segment != null)
				{
					// split apart any strings with a '.' and remove any consecutive multiple '.'
					var segments = segment.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

					// being we are making a namespace, make sure the segments are valid
					var validSegments = segments.AsValidWords();
					joinedNamespace.AddRange(validSegments);
				}
			}

			var ns = string.Join(".", joinedNamespace.ToArray());
			return ns;
		}

		/// <summary>
		/// Remplacement des caractères accentués
		/// </summary>
		/// <param name="s">The string to replace diacritics on.</param>
		/// <remarks>A diacritic is a glyph added to a letter, or basic glyph</remarks>
		/// <returns>The string without diacritic marks.</returns>
		private static string RemoveDiacritics(string s)
		{
			var normalizedString = s.Normalize(NormalizationForm.FormD);
			var stringBuilder = new StringBuilder();

			foreach (var c in normalizedString)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Tests whether the word conflicts with reserved or language keywords, and if so, attempts to return a 
		/// valid word that does not conflict. Usually the returned word is only slightly modified to differentiate 
		/// the identifier from the keyword; for example, the word might be preceded by the underscore ("_") character.
		/// </summary>
		/// <param name="word">The word.</param>
		/// <returns>A valid identifier.</returns>
		private static string FixReservedWord(string word)
		{
			// turns keywords into usable words.
			// i.e. class -> _class
			var codeProvider = new Microsoft.CSharp.CSharpCodeProvider();
			return codeProvider.CreateValidIdentifier(word);
		}

		/// <summary>
		/// The get formatted word.
		/// </summary>
		/// <param name="word">
		/// The word.
		/// </param>
		/// <param name="transformations">
		/// The transformations.
		/// </param>
		/// <returns>
		/// The formatted version of the original string.
		/// </returns>
		private static string GetFormattedWord(this string word, params Func<string, string>[] transformations)
		{
			var newWord = word;
			foreach (var item in transformations)
			{
				if (item != null)
				{
					newWord = item(newWord);
				}
			}

			// Now that the basic transforms are done, make sure we have a valid word to use 
			newWord = newWord.AsValidWord();
			return newWord;
		}
	}
}
