using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OpenQA.Selenium;

namespace DasBlog.Tests.Functional.Selenium
{
	public class TestFixture : IDisposable
	{
		private Process _process;
		public IWebDriver WebDriver;


		public TestFixture()
		{
			StartServer();
		}

		private void StartServer()
		{
			string projectname = "";
			string applicationPath = "";

			_process = new Process
			{

			};

			_process.Start();
			// WebDriver = W
		}

		public void Dispose()
		{
			WebDriver.Dispose();
			if (!_process.HasExited)
			{
				_process.Kill();
			}
		}
	}
}
