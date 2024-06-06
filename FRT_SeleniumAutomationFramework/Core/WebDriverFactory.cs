using Strypes_SeleniumAutomationFramework.Properties;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System.Drawing;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using YamlDotNet.Serialization;

namespace Strypes_SeleniumAutomationFramework.Core
{
    public class WebDriverFactory
    {

        public IWebDriver CreateBrowser(Network type, string browserName, string version, string os, string name, string ltUserName, string ltAccessKey, bool isHeadless)
        {
            return type switch
            {
                Network.Local => browserName switch
                {
                    "Chrome" => GetChromeDriver(isHeadless),
                    "Firefox" => GetFirefoxDriver(isHeadless),
                    _ => throw new ArgumentOutOfRangeException(browserName.ToString(), $"No such browser {browserName}")
                },
                Network.Remote => browserName switch
                {
                    "Chrome" => CreateLambdaTestChromeDriver(version, os, name, ltUserName, ltAccessKey),
                    "Firefox" => CreateLambdaTestFirefoxDriver(version, os, name, ltUserName, ltAccessKey),
                    _ => throw new ArgumentOutOfRangeException(type.ToString(), $"Unknown Environment {type}")
                },

            };
        }

        public IWebDriver GetFirefoxDriver(bool isHeadless)
        {
            var options = new FirefoxOptions();

            if (isHeadless)
            {
                options.AddArgument("--headless");
                new DriverManager().SetUpDriver(new FirefoxConfig());
            }

            IWebDriver driver = new FirefoxDriver(options);
            if (!isHeadless)
            {
                driver.Manage().Window.Size = new Size(1920, 1080);
                driver.Manage().Window.Maximize();

            }
            return driver;
        }

        public IWebDriver GetFirefoxDriver()
        {
            var options = new FirefoxOptions();
            new DriverManager().SetUpDriver(new FirefoxConfig());

            IWebDriver driver = new FirefoxDriver(options);
            driver.Manage().Window.Maximize();

            return driver;
        }

        public IWebDriver GetChromeDriver(bool isHeadless)
        {
            var options = new ChromeOptions();

            if (isHeadless)
            {
                options.AddArgument("--headless");

            }
            if (!isHeadless)
            {
                options.AddArgument($"--window-size=1920,1080");
            }

            new DriverManager().SetUpDriver(new ChromeConfig());

            IWebDriver driver = new ChromeDriver(options);

            return driver;
        }
        public IWebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();
            new DriverManager().SetUpDriver(new ChromeConfig());
            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            //((IJavaScriptExecutor)driver).ExecuteScript($"window.resizeTo(1366, 668);");
            //options.AddArgument("--start-maximized");
            //options.AddArgument($"--width={windowWidth}");
            //options.AddArgument($"--height={windowHeight}");

            return driver;
        }

        public IWebDriver GetEdgeDriver()
        {
            var options = new EdgeOptions();

            //options.AddArgument("--start-maximized");

            new DriverManager().SetUpDriver(new EdgeConfig());
            return new EdgeDriver(options);
        }
        public IWebDriver CreateLambdaTestChromeDriver(string version, string os, string name, string userName, string appKey)
        {
            var options = new ChromeOptions();
            //options.AddArgument("--start-maximized");
            options.AddArgument("--window-size=1920,1080");

            options.AddAdditionalOption("platform", os);
            options.AddAdditionalOption("version", version);
            options.AddAdditionalOption("name", name);
            options.AddAdditionalOption("build", "Parallel Browser Testing");
            options.AddAdditionalOption("user", userName);
            options.AddAdditionalOption("accessKey", appKey);

            return new RemoteWebDriver(
                new Uri("https://" + userName + ":" + appKey + "@hub.lambdatest.com/wd/hub"),
                options.ToCapabilities(), TimeSpan.FromSeconds(600));
        }

        public IWebDriver CreateLambdaTestFirefoxDriver(string version, string os, string name, string userName, string appKey)
        {
            var options = new FirefoxOptions();


            //options.AddArgument("--start-maximized");

            //it was working with those with the first tests
            options.AddArgument("--width=1920");
            options.AddArgument("--height=1080");
            //options.AddAdditionalOption("resolution", "1920x1080");
            options.AddAdditionalOption("platform", os);
            options.AddAdditionalOption("version", version);
            options.AddAdditionalOption("name", name);
            options.AddAdditionalOption("build", "Parallel Browser Testing");
            options.AddAdditionalOption("user", userName);
            options.AddAdditionalOption("accessKey", appKey);

            return new RemoteWebDriver(
                new Uri("https://" + userName + ":" + appKey + "@hub.lambdatest.com/wd/hub"),
                options.ToCapabilities(), TimeSpan.FromSeconds(600));
        }


    }
}
