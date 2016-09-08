/*
 * $Id: GzipModule.cs 439 2007-11-26 13:26:10Z spocke $
 *
 * @author Moxiecode
 * @copyright Copyright © 2004-2007, Moxiecode Systems AB, All rights reserved.
 */

using System;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using Moxiecode.TinyMCE.Compression;

namespace Moxiecode.TinyMCE.Compression {
	/// <summary>
	/// Description of HttpHandler.
	/// </summary>
	public class GzipModule : Moxiecode.TinyMCE.Web.IModule {
		/// <summary></summary>
		/// <param name="context">Request context.</param>
		public void ProcessRequest(HttpContext context) {
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			HttpServerUtility server = context.Server;
			GzipCompressor gzipCompressor = new GzipCompressor();
			ConfigSection configSection = (ConfigSection) System.Web.HttpContext.Current.GetSection("TinyMCE");
			string suffix = "", enc;
			string[] languages = request.QueryString["languages"].Split(',');
			bool supportsGzip;

			// Set response headers
			response.ContentType = "text/javascript";
			response.Charset = "UTF-8";
			response.Buffer = false;

			// Setup cache
			response.Cache.SetExpires(DateTime.Now.AddTicks(configSection.GzipExpiresOffset));
			response.Cache.SetCacheability(HttpCacheability.Public);
			response.Cache.SetValidUntilExpires(false);

			// Check if it supports gzip
			enc = Regex.Replace("" + request.Headers["Accept-Encoding"], @"\s+", "").ToLower();
			supportsGzip = enc.IndexOf("gzip") != -1 || request.Headers["---------------"] != null;
			enc = enc.IndexOf("x-gzip") != -1 ? "x-gzip" : "gzip";

			// Handle mode/suffix
			if (configSection.Mode != null)
				suffix = "_" + configSection.Mode;

			gzipCompressor.AddData("var tinyMCEPreInit = {base : '" + new Uri(request.Url, configSection.InstallPath).ToString() + "', suffix : '" + suffix + "'};");

			// Add core
			gzipCompressor.AddFile(server.MapPath(configSection.InstallPath + "/tiny_mce" + suffix + ".js"));

			// Add core languages
			foreach (string lang in languages)
				gzipCompressor.AddFile(server.MapPath(configSection.InstallPath + "/langs/" + lang + ".js"));

			// Add themes
			if (request.QueryString["themes"] != null) {
				foreach (string theme in request.QueryString["themes"].Split(',')) {
					gzipCompressor.AddFile(server.MapPath(configSection.InstallPath + "/themes/" + theme + "/editor_template" + suffix + ".js"));

					// Add theme languages
					foreach (string lang in languages) {
						string path = server.MapPath(configSection.InstallPath + "/themes/" + theme + "/langs/" + lang + ".js");

						if (File.Exists(path))
							gzipCompressor.AddFile(path);
					}

					gzipCompressor.AddData("tinymce.ThemeManager.urls['" + theme + "'] = tinymce.baseURL+'/themes/" + theme + "';");
				}
			}

			// Add plugins
			if (request.QueryString["plugins"] != null) {
				foreach (string plugin in request.QueryString["plugins"].Split(',')) {
					gzipCompressor.AddFile(server.MapPath(configSection.InstallPath + "/plugins/" + plugin + "/editor_plugin" + suffix + ".js"));

					// Add plugin languages
					foreach (string lang in languages) {
						string path = server.MapPath(configSection.InstallPath + "/plugins/" + plugin + "/langs/" + lang + ".js");

						if (File.Exists(path))
							gzipCompressor.AddFile(path);
					}

					gzipCompressor.AddData("tinymce.ThemeManager.urls['" + plugin + "'] = tinymce.baseURL+'/plugins/" + plugin + "';");
				}
			}

			// Output compressed file
			gzipCompressor.NoCompression = !supportsGzip || configSection.GzipNoCompression;
			if (!gzipCompressor.NoCompression)
				response.AppendHeader("Content-Encoding", enc);

			gzipCompressor.DiskCache = configSection.GzipDiskCache;
			gzipCompressor.CachePath = configSection.GzipCachePath;

			gzipCompressor.Compress(response.OutputStream);
		}
	}
}
