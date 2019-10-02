using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DasBlog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace DasBlog.Tests.Functional.Selenium
{
	public class SeleniumServerFactory<TStartup> : WebApplicationFactory<Startup> where TStartup : class
	{
		public string RootUri { get; set; } //Save this use by tests

		Process _process;
		IWebHost _host;

		public SeleniumServerFactory()
		{
			ClientOptions.BaseAddress = new Uri("https://localhost"); //will follow redirects by default
			RootUri = "https://localhost:5001";

			_process = new Process()
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "selenium-standalone",
					Arguments = "start",
					UseShellExecute = true
				}
			};
			_process.Start();
		}

		protected override TestServer CreateServer(IWebHostBuilder builder)
		{
			//Real TCP port
			_host = builder.Build();
			_host.Start();
			RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault(); //Last is https://localhost:5001!

			//Fake Server we won't use...this is lame. Should be cleaner, or a utility class
			return new TestServer(new WebHostBuilder().UseStartup<TStartup>());
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_host.Dispose();
				_process.CloseMainWindow(); //Be sure to stop Selenium Standalone
			}
		}
	}
}
