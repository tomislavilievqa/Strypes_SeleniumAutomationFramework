using Strypes_SeleniumAutomationFramework.Properties;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strypes_SeleniumAutomationFramework.PageObjects
{
    public class NavigationBar : BasePage
    {
        public NavigationBar(IWebDriver driver, CurrentEnvironment environment) : base(driver, environment)
        {
        }
        public IWebElement ContactUsButton => WaitAndFindElement(By.XPath("//*[@id=\"menu-1-76b7430b\"]/li[2]/a"));

    }
}
