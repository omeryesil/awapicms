/*
 * Created by SharpDevelop.
 * User: spocke
 * Date: 2007-11-23
 * Time: 13:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Moxiecode.TinyMCE.SpellChecker {
	/// <summary>
	/// Description of ISpellChecker.
	/// </summary>
	public interface ISpellChecker {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lang"></param>
		/// <param name="words"></param>
		/// <returns></returns>
		string[] CheckWords(string lang, string[] words);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lang"></param>
		/// <param name="word"></param>
		/// <returns></returns>
		string[] GetSuggestions(string lang, string word);
	}
}
