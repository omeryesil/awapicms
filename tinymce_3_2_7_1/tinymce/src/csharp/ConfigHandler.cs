/*
 * $Id: ConfigHandler.cs 439 2007-11-26 13:26:10Z spocke $
 *
 * @author Moxiecode
 * @copyright Copyright © 2004-2007, Moxiecode Systems AB, All rights reserved.
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Text.RegularExpressions;

namespace Moxiecode.TinyMCE {
	/// <summary>
	///  Handles configuration specific for the manager and it's subcomponents.
	/// </summary>
	public class ConfigHandler : IConfigurationSectionHandler {
		object IConfigurationSectionHandler.Create(object parent, object config_context, XmlNode section) {
			ConfigSection configSection = new ConfigSection();
			NameValueCollection config = configSection.GlobalSettings;
			XmlElement gzipCompressorElm;

			// Get core values
			configSection.InstallPath = ((XmlElement) section).GetAttribute("installPath");
			configSection.Mode = ((XmlElement) section).GetAttribute("mode");

			// Parse and setup global settings
			foreach (XmlElement addElm in section.SelectNodes("globalSettings/add"))
				config.Add(addElm.GetAttribute("key"), addElm.GetAttribute("value"));

			// Parse and setup gzip
			gzipCompressorElm = (XmlElement) section.SelectSingleNode("gzipCompressor");
			if (gzipCompressorElm != null)
				configSection.GzipEnabled = gzipCompressorElm.GetAttribute("enabled") == "yes";

			// Disk cache
			gzipCompressorElm = (XmlElement) section.SelectSingleNode("gzipCompressor");
			if (gzipCompressorElm != null)
				configSection.GzipDiskCache = gzipCompressorElm.GetAttribute("diskCache") == "yes";

			// Cache path
			gzipCompressorElm = (XmlElement) section.SelectSingleNode("gzipCompressor");
			if (gzipCompressorElm != null)
				configSection.GzipCachePath = gzipCompressorElm.GetAttribute("cachePath");

			// No compression
			gzipCompressorElm = (XmlElement) section.SelectSingleNode("gzipCompressor");
			if (gzipCompressorElm != null)
				configSection.GzipNoCompression = gzipCompressorElm.GetAttribute("noCompression") == "yes";

			// Setup expires offset
			if (configSection.GzipEnabled) {
				string expiresOffset = gzipCompressorElm.GetAttribute("expiresOffset");
				TimeSpan timeSpan;
				Match match;

				if (expiresOffset == null)
					expiresOffset = "10d";

				match = Regex.Match(expiresOffset, @"^([0-9]+)([dwmy]?)$");

				switch (match.Groups[2].Value) {
					case "d":
						timeSpan = TimeSpan.FromDays(Convert.ToInt32(match.Groups[1].Value));
						break;

					case "w":
						timeSpan = TimeSpan.FromDays(Convert.ToInt32(match.Groups[1].Value) * 7);
						break;

					case "m":
						timeSpan = TimeSpan.FromDays(Convert.ToInt32(match.Groups[1].Value)* 30);
						break;

					case "y":
						timeSpan = TimeSpan.FromDays(Convert.ToInt32(match.Groups[1].Value) * 365);
						break;

					default:
						timeSpan = TimeSpan.FromTicks(Convert.ToInt32(match.Groups[1].Value));
						break;
				}

				configSection.GzipExpiresOffset = timeSpan.Ticks;
			}

			return configSection;
		}
	}
}
