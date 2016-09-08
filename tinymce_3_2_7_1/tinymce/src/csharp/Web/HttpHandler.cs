/*
 * $Id: HttpHandler.cs 439 2007-11-26 13:26:10Z spocke $
 *
 * @author Moxiecode
 * @copyright Copyright © 2004-2007, Moxiecode Systems AB, All rights reserved.
 */

using System;
using System.Web;
using Moxiecode.TinyMCE.Compression;

namespace Moxiecode.TinyMCE.Web {
	/// <summary>
	/// Description of HttpHandler.
	/// </summary>
	public class HttpHandler : System.Web.IHttpHandler {
		/// <summary>
		/// 
		/// </summary>
		public HttpHandler() {
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReusable {
			get {
				return true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context) {
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			IModule module = null;

			// This might include some form of dynamic class loading later
			switch (request.QueryString["module"]) {
				case "GzipModule":
					module = new Moxiecode.TinyMCE.Compression.GzipModule();
					break;

				case "SpellChecker":
					module = new Moxiecode.TinyMCE.SpellChecker.SpellCheckerModule();
					break;
			}

			if (module != null)
				module.ProcessRequest(context);
		}
	}
}
