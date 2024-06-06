using OpenQA.Selenium;
using Strypes_SeleniumAutomationFramework.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strypes_SeleniumAutomationFramework.PageObjects
{
    public class Homepage : BasePage
    {
        public override string pageTitleTesting => "Testing Strypes | End-to-end software solutions";
        public override string pageTitleStaging => "Staging Strypes | End-to-end software solutions";
        public override string pageTitleLive => "Strypes | End-to-end software solutions";
        public override string pageUrlTesting => $"https://{environment.BaseUrl}/careers/";
        public override string pageUrlStaging => $"https://{environment.BaseUrl}/careers/";
        public override string pageUrlLive => $"https://{environment.BaseUrl}/careers/";

        public Homepage(IWebDriver driver, CurrentEnvironment environment) : base(driver, environment)
        {
        }

    }
}
