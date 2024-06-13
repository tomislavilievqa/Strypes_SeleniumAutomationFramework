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
        private static ExtentReports _extentReports;
        private static ExtentHtmlReporter _htmlReporter;
        private ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();
        private WebDriverFactory _factory;

        protected IWebDriver Driver => _driver.Value;
        protected EnvironmentManager _manager { get; private set; }
        protected CurrentEnvironment environment { get; private set; }
        protected BrowserType _currentBrowserType => (BrowserType)TestContext.CurrentContext.Test.Arguments[1];

        private readonly string _browser;
        private readonly string _version;
        private readonly string _os;
        private readonly string _name;
        private readonly bool _isHeadless = true;
        private string _ltUserName;
        private string _ltAccessKey;

        public static ThreadLocal<ExtentTest> _test = new ThreadLocal<ExtentTest>();

        protected string _testName => $"{_className}_{TestContext.CurrentContext.Test.MethodName}";
        protected string _className => TestContext.CurrentContext.Test.ClassName.Split('.', StringSplitOptions.RemoveEmptyEntries)[2];
        protected string _baseUrl => $"https://{environment.BaseUrl}/";

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
            InitializeEnvironment();
            InitializeReport();
        }

        [SetUp]
        public void BeforeTest()
        {
            InitializeWebDriver();
            StartTestReport();
        }

        [TearDown]
        public void AfterTest()
        {
            EndTestReport();
            DisposeDriver();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            _extentReports.Flush();
        }

        private void InitializeEnvironment()
        {
            _manager = new EnvironmentManager();
            _manager.SetEnvironment("Production");
            environment = _manager.GetCurrentEnvironment();
            _ltUserName = environment.ltUserName;
            _ltAccessKey = environment.ltAccessKey;
        }

        private void InitializeReport()
        {
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
            _extentReports.AddSystemInfo("Environment", $"{environment}");
            _extentReports.AddSystemInfo("Tester", Environment.UserName);
            _extentReports.AddSystemInfo("MachineName", Environment.MachineName);
        }

        private void InitializeWebDriver()
        {
            _factory = new WebDriverFactory();
            _driver.Value = _factory.CreateBrowser(Network.Local, _browser, _version, _os, _name, _ltUserName, _ltAccessKey, !_isHeadless);
        }

        private void StartTestReport()
        {
            _test.Value = _extentReports.CreateTest($"{TestContext.CurrentContext.Test.MethodName}_{_browser}").Info("Test Started");
            _test.Value.AssignCategory(_className);
            _test.Value.AssignAuthor("Tomislav Iliev");
        }

        private void EndTestReport()
        {
            var extentTest = _test.Value;

            switch (TestContext.CurrentContext.Result.Outcome.Status)
            {
                case NUnit.Framework.Interfaces.TestStatus.Passed:
                    extentTest.Pass("Test passed");
                    break;
                case NUnit.Framework.Interfaces.TestStatus.Failed:
                    extentTest.Fail("Test failed");
                    break;
                default:
                    extentTest.Skip("Test skipped");
                    break;
            }
        }

        private void DisposeDriver()
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
