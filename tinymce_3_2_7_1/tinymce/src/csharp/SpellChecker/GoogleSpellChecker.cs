/*
 * Created by SharpDevelop.
 * User: spocke
 * Date: 2007-11-23
 * Time: 13:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Net;
using System.Xml;

namespace Moxiecode.TinyMCE.SpellChecker {
	/// <summary>
	/// Description of GoogleSpellChecker.
	/// </summary>
	public class GoogleSpellChecker : ISpellChecker {
		/// <summary>
		/// Description of GoogleSpellChecker.
		/// </summary>
		public GoogleSpellChecker() {
		}

		/// <summary>
		///  
		/// </summary>
		/// <param name="lang"></param>
		/// <param name="words"></param>
		/// <returns></returns>
		public string[] CheckWords(string lang, string[] words) {
			ArrayList misspelledWords = new ArrayList();
			XmlDocument doc = new XmlDocument();
			string result, wordsStr;

			// Send request to google
			wordsStr = String.Join(" ", words);
			result = this.SendRequest(lang, wordsStr);

			// Parse XML result
			doc.LoadXml(result);

			// Build misspelled word list
			foreach (XmlNode node in doc.SelectNodes("//c")) {
				XmlElement cElm = (XmlElement) node;

				misspelledWords.Add(wordsStr.Substring(Convert.ToInt32(cElm.GetAttribute("o")), Convert.ToInt32(cElm.GetAttribute("l"))));
			}

			return (string[]) misspelledWords.ToArray(typeof(string));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lang"></param>
		/// <param name="word"></param>
		/// <returns></returns>
		public string[] GetSuggestions(string lang, string word) {
			ArrayList suggestions = new ArrayList();
			XmlDocument doc = new XmlDocument();
			string result;

			// Send request to google
			result = this.SendRequest(lang, word);

			// Parse XML result
			doc.LoadXml(result);

			// Build misspelled word list
			foreach (XmlNode node in doc.SelectNodes("//c")) {
				XmlElement cElm = (XmlElement) node;

				foreach (string sug in cElm.InnerText.Split('\t')) {
					if (sug != "")
						suggestions.Add(sug);
				}
			}

			return (string[]) suggestions.ToArray(typeof(string));
		}

		#region private

		private string SendRequest(string lang, string data) {
			string server = "www.google.com";
			string port = "443";
            string path = "/tbproxy/spell?lang=" + lang + "&hl=" + lang;
			string url = "https://" + server + ":" + port + path;
			string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?><spellrequest textalreadyclipped=\"0\" ignoredups=\"0\" ignoredigits=\"1\" ignoreallcaps=\"1\"><text>" + this.EntityEncode(data) + "</text></spellrequest>";
			string outData;
			StreamReader resStream = null;
			HttpWebResponse res = null;
			Stream reqStream = null;

			try {
				HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
				req.KeepAlive = false;
				req.Method = "POST";
				req.ContentType = "application/PTI26";
				req.ContentLength = xml.Length;

				// Google-specific headers
				WebHeaderCollection reqHeaders = req.Headers;
				reqHeaders.Add("MIME-Version: 1.0");
				reqHeaders.Add("Request-number: 1");
				reqHeaders.Add("Document-type: Request");
				reqHeaders.Add("Interface-Version: Test 1.4");
				//reqHeaders.Add("Connection: Close");
				reqStream = req.GetRequestStream();

				ASCIIEncoding encoding = new ASCIIEncoding();
				byte[] xmlData = encoding.GetBytes(xml);
				reqStream.Write(xmlData, 0, xmlData.Length);

				res = (HttpWebResponse) req.GetResponse();
				resStream = new StreamReader(res.GetResponseStream());
				outData = resStream.ReadToEnd();
			} finally {
				if (reqStream != null)
					reqStream.Close();

				if (resStream != null)
					resStream.Close();

				if (res != null)
					res.Close();
			}
				
			return outData;
		}

		private string EntityEncode(string str) {
			StringBuilder buff = new StringBuilder();

			for (int i=0; i<str.Length; i++) {
				char chr = str[i];

				if (chr > 127) {
					buff.Append("&#");
					buff.Append((int) chr);
					buff.Append(';');
				} else
					buff.Append(chr);
			}

			return buff.ToString();
		}
		
		#endregion
	}
}
