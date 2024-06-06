using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Strypes_SeleniumAutomationFramework.Core;
using Strypes_SeleniumAutomationFramework.Properties;
using Strypes_SeleniumAutomationFramework.Utilities;
using Microsoft.Extensions.Configuration.UserSecrets;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;


[assembly: Parallelizable(ParallelScope.All)]
[assembly: LevelOfParallelism(4)]
namespace Strypes_SeleniumAutomationFramework.Tests
{
    public class BaseTest : IDisposable
    {
        private ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();
        protected IWebDriver Driver => _driver.Value;
        private static ExtentReports _extentReports;
        private static ExtentHtmlReporter _htmlReporter;
        public static ThreadLocal<ExtentTest> _test = new ThreadLocal<ExtentTest>();
        protected EnvironmentManager _manager { get; set; }
        private WebDriverFactory _factory { get; set; }
        protected CurrentEnvironment environment { get; set; }
        protected readonly string _browser;
        private readonly string _version;
        private readonly string _os;
        private readonly string _name;
        private bool _isHeadless;
        private string _ltUserName;
        private string _ltAccessKey;
        protected BrowserType _currentBrowserType => (BrowserType)TestContext.CurrentContext.Test.Arguments[1];
        public string _testName => _className + "_" + TestContext.CurrentContext.Test.MethodName;
        public string _className => TestContext.CurrentContext.Test.ClassName.Split('.', StringSplitOptions.RemoveEmptyEntries)[2];
        protected string _baseUrl => $"https://{this.environment.BaseUrl}/";

        public BaseTest(string browser, string version, string os, string name)
        {
            _isHeadless = true;
            _version = version;
            _os = os;
            _name = name;
            _browser = browser;

        }

        [OneTimeSetUp]
        public void TestSetup()
        {          
            // ** Selecting the environment on which we want to execute the TC **
            _manager = new EnvironmentManager();
            _manager.SetEnvironment("Production");
            this.environment = _manager.GetCurrentEnvironment();
            _ltUserName = this.environment.ltUserName;
            _ltAccessKey = this.environment.ltAccessKey;

            string path = Assembly.GetCallingAssembly().CodeBase;
            string actualPath = path.Substring(0, path.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            string reportPath = Path.Combine(projectPath, "Reports", GetType().Name);

            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }

            _htmlReporter = new ExtentHtmlReporter(Path.Combine(reportPath, "index.html"));

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(_htmlReporter);
            _extentReports.AddSystemInfo("Environment", $"{this.environment}");
            _extentReports.AddSystemInfo("Tester", Environment.UserName);
            _extentReports.AddSystemInfo("MachineName", Environment.MachineName);
        }

        [SetUp]
        public void BeforeTest()
        {
            _factory = new WebDriverFactory();

            // Initialize WebDriver in a thread-safe manner
            _driver.Value = _factory.CreateBrowser(Network.Local, _browser, _version, _os, _name, _ltUserName, _ltAccessKey, !_isHeadless);

            _test.Value = _extentReports.CreateTest($"{TestContext.CurrentContext.Test.MethodName}_{_browser}").Info("Test Started");
            _test.Value.AssignCategory(_className);
            _test.Value.AssignAuthor("Tomislav Iliev");
        }

        [TearDown]
        public void AfterTest()
        {
            var extentTest = _test.Value;

            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                extentTest.Pass("Test passed");
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                extentTest.Fail("Test failed");
            }
            else
            {
                extentTest.Skip("Test skipped");
            }

            Dispose();

        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            _extentReports.Flush();

        }

        public void Dispose()
        {
            if (_driver.IsValueCreated)
            {
                try
                {
                    Driver.Quit();
                    Driver.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during WebDriver disposal: {ex.Message}");
                }
                finally
                {
                    _driver.Value = null;
                }
            }
        }

    }
}
