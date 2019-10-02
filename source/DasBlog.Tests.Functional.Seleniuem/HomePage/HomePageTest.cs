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
			Server.RootUri = "https://localhost:5001/";
			Client = server.CreateClient(); //weird side effecty thing here. This call shouldn't be required for setup, but it is.

			var opts = new ChromeOptions();
			//opts.AddArgument("--headless"); //Optional, comment this out if you want to SEE the browser window
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
		public void ThereIsAnH2()
		{
			Browser.Navigate().GoToUrl(Server.RootUri);

			var headerSelector = By.TagName("h2");
			Assert.Equal("Congratulations, you've installed dasBlog!", Browser.FindElement(headerSelector).Text);
		}

		[Fact]
		[Trait("Home", "FunctionalTest")]
		public void GoToTheFirstBlogPostThenBackHome()
		{
			Browser.Navigate().GoToUrl(Server.RootUri + "/congratulations-youve-installed-dasblog");

			var headerSelector = By.TagName("h2");
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
