using System;
using System.Net.Http;
using DasBlog.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xunit;

namespace DasBlog.Tests.Functional.Selenium
{
	public class HomePageTest : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
	{
		public SeleniumServerFactory<Startup> Server { get; }
		public IWebDriver Browser { get; }
		public HttpClient Client { get; }
		public ILogs Logs { get; }

		public HomePageTest(SeleniumServerFactory<Startup> server)
		{
			Server = server;
			Client = server.CreateClient(); //weird side effecty thing here. This call shouldn't be required for setup, but it is.

			var opts = new ChromeOptions();
			opts.AddArgument("--headless"); //Optional, comment this out if you want to SEE the browser window
			opts.SetLoggingPreference(OpenQA.Selenium.LogType.Browser, LogLevel.All);

			var driver = new RemoteWebDriver(opts);
			Browser = driver;
			Logs = new RemoteLogs(driver); //TODO: Still not bringing the logs over yet
		}

		[Fact]
		[Trait("Home", "FunctionalTest")]
		public void LoadTheMainPageAndCheckTitle()
		{
			Browser.Navigate().GoToUrl(Server.RootUri);
			Assert.StartsWith("My DasBlog!", Browser.Title);
		}

		[Fact]
		[Trait("Home", "FunctionalTest")]
		public void ThereIsAnH1()
		{
			Browser.Navigate().GoToUrl(Server.RootUri);

			var headerSelector = By.TagName("h1");
			Assert.Equal("HANSELMINUTES PODCAST\r\nby Scott Hanselman", Browser.FindElement(headerSelector).Text);
		}

		[Fact]
		[Trait("Home", "FunctionalTest")]
		public void KevinScottTestThenGoHome()
		{
			Browser.Navigate().GoToUrl(Server.RootUri + "/631/how-do-you-become-a-cto-with-microsofts-cto-kevin-scott");

			var headerSelector = By.TagName("h1");
			var link = Browser.FindElement(headerSelector);
			link.Click();
			Assert.Equal(Browser.Url.TrimEnd('/'), Server.RootUri); //WTF
		}

		public void Dispose()
		{
			Browser.Dispose();
		}
	}
}
