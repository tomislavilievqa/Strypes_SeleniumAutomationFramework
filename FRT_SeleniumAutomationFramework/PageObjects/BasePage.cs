using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using Strypes_SeleniumAutomationFramework.Properties;
using Strypes_SeleniumAutomationFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strypes_SeleniumAutomationFramework.PageObjects
{
    public class BasePage
    {
        protected IWebDriver driver;
        protected CurrentEnvironment environment;
        private Actions action;
        private DateTime initDateTimeFormat => DateTime.Now;

        public virtual string pageUrl { get; }
        public virtual string pageTitleTesting { get; }
        public virtual string pageTitleStaging { get; }
        public virtual string pageTitleLive { get; }
        public virtual string pageUrlTesting { get; }
        public virtual string pageUrlStaging { get; }
        public virtual string pageUrlLive { get; }

        public string pageUrlExtensionTesting => "Flatrock QA";
        public string pageUrlExtensionStaging => "Flat Rock Technology - UAT";
        public string pageUrlExtensionLive => "Flat Rock Technology";


        public BasePage(IWebDriver driver, CurrentEnvironment environment)
        {
            this.driver = driver;
            this.environment = environment;
        }

        public string GetExpectedUrlBasedOnEnvironment(CurrentEnvironment environment)
        {
            if (environment.BaseUrl.Contains("flatrocktech-frontend-qa"))
            {
                return pageUrlTesting;
            }
            else if (environment.BaseUrl.Contains("frontend."))
            {
                return pageUrlStaging;
            }
            else if (environment.BaseUrl.Contains("flatrocktech.com"))
            {
                return pageUrlLive;
            }
            else
            {
                throw new ArgumentException("Unsupported environment");
            }

        }

        public string GetExpectedTitleBasedOnEnvironment(CurrentEnvironment environment)
        {

            if (environment.BaseUrl.Contains("flatrocktech-frontend-qa"))
            {
                return pageTitleTesting;
            }
            else if (environment.BaseUrl.Contains("frontend."))
            {
                return pageTitleStaging;
            }
            else if (environment.BaseUrl == "flatrocktech.com")
            {
                return pageTitleLive;
            }
            else
            {
                throw new ArgumentException("Unsupported environment");
            }

        }

        public void ScrollToButton(IWebElement button)
        {
            WaitUntilElementIsClickable(button);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", button);
            HighlightElement(button);
        }

        public void ScrollToButton(IWebElement button, int yOffset)
        {
            WaitUntilElementIsClickable(button);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", button);
            jsExecutor.ExecuteScript("window.scrollBy(0, arguments[1]);", yOffset);
            HighlightElement(button);
        }

        public void ScrollToButton(IList<IWebElement> button, int elementNum)
        {
            WaitUntilElementIsClickable(button[elementNum]);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", button[elementNum]);
        }

        public void ScrollToButton(IList<IWebElement> button, int elementNum, int yOffset)
        {
            WaitUntilElementIsClickable(button[elementNum]);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("window.scrollBy(0, arguments[1]);", yOffset);
            jsExecutor.ExecuteScript("arguments[0].scrollIntoView(true);", button[elementNum]);
        }

        public void ScrollToByCoordinates(IWebElement element)
        {
            ElementCoordinates elementCoordinates = new ElementCoordinates();
            WaitUntilElementIsClickable(element);
            int xCoordinate = elementCoordinates.GetElementXCoordinate(element);
            int yCoordinate = elementCoordinates.GetElementYCoordinate(element);

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("window.scrollTo(arguments[0], arguments[1]);", xCoordinate - 200, yCoordinate - 200);
        }


        public void WaitForElementToLoad(By name, int duration = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(duration));
                wait.Until(ExpectedConditions.ElementIsVisible(name));
            }
            catch
            {
                TakeScreenshot();
                Console.WriteLine($"Timeout waiting for element to load");
            }

        }

        public void WaitForTextToBePresent(IWebElement element, string text, int duration = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(duration));
                wait.Until(ExpectedConditions.TextToBePresentInElement(element, text));
                HighlightElement(element);
            }
            catch (WebDriverTimeoutException ex)
            {
                TakeScreenshot();
                Console.WriteLine($"Timeout waiting for text '{text}' in element. Exception details: {ex.Message}");
            }

        }

        public void WaitUntilElementIsClickable(IWebElement element)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
                HighlightElement(element);
            }
            catch (WebDriverTimeoutException ex)
            {
                TakeScreenshot();
                Console.WriteLine($"Timeout waiting for the element to be clickable. Exception details: {ex.Message}");
            }

        }

        public IWebElement WaitAndFindElement(By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));

            return wait.Until(ExpectedConditions.ElementExists(locator));
        }
        public bool IsElementDisplayed(By by)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException)); // Ignore NoSuchElementException

                return wait.Until(driver =>
                {
                    var element = driver.FindElement(by);
                    return element.Displayed;
                });
            }
            catch
            {
                return false;
            }
        }

        public IWebElement WaitUntilElementIsClickable(By element)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.IgnoreExceptionTypes(typeof(ElementClickInterceptedException));
                return wait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (WebDriverTimeoutException ex)
            {
                TakeScreenshot();
                throw new WebDriverTimeoutException($"Timeout waiting for element to be clickable. {ex.Message}");
            }

        }

        public IList<IWebElement> WaitUntilElementsAreClickable(By element)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
                return driver.FindElements(element);

            }
            catch (WebDriverTimeoutException ex)
            {
                TakeScreenshot();
                throw new WebDriverTimeoutException($"Timeout waiting for element to be clickable. {ex.Message}");
            }

        }

        public IList<IWebElement> WaitAndFindElements(By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
            //return driver.FindElements(locator);
        }

        public IWebElement LocateElement(Locators type, string name)
        {
            return type switch
            {
                Locators.Xpath => this.driver.FindElement(By.XPath(name)),
                Locators.CssSelector => this.driver.FindElement(By.CssSelector(name)),
                Locators.ID => this.driver.FindElement(By.Id(name)),
                Locators.Name => this.driver.FindElement(By.Name(name)),
                Locators.LinkText => this.driver.FindElement(By.LinkText(name)),
                Locators.ClassName => this.driver.FindElement(By.ClassName(name)),
                Locators.PartialLinkText => this.driver.FindElement(By.PartialLinkText(name)),
                Locators.TagName => this.driver.FindElement(By.TagName(name)),
                _ => throw new ArgumentOutOfRangeException(type.ToString(), $"Invalid Type, {type.ToString()}")
            };
        }

        public IList<IWebElement> LocateElements(Locators type, string name)
        {
            return type switch
            {
                Locators.Xpath => this.driver.FindElements(By.XPath(name)),
                Locators.CssSelector => this.driver.FindElements(By.CssSelector(name)),
                Locators.ID => this.driver.FindElements(By.Id(name)),
                Locators.Name => this.driver.FindElements(By.Name(name)),
                Locators.LinkText => this.driver.FindElements(By.LinkText(name)),
                Locators.ClassName => this.driver.FindElements(By.ClassName(name)),
                Locators.PartialLinkText => this.driver.FindElements(By.PartialLinkText(name)),
                Locators.TagName => this.driver.FindElements(By.TagName(name)),
                _ => throw new ArgumentOutOfRangeException(type.ToString(), $"Invalid Type, {type.ToString()}")
            };
        }

        public void SwitchToNeighborTab()
        {
            List<string> windowHandles = new List<string>(driver.WindowHandles);

            if (windowHandles.Count >= 2)
            {
                driver.SwitchTo().Window(windowHandles[1]);
            }
            else
            {
                TakeScreenshot();
                throw new InvalidOperationException("There are not enough tabs to switch to.");
            }
        }

        public void SwitchToOriginalTab()
        {
            List<string> windowHandles = new List<string>(driver.WindowHandles);

            if (windowHandles.Count >= 2)
            {
                driver.SwitchTo().Window(windowHandles[0]);
            }
            else
            {
                TakeScreenshot();
                throw new InvalidOperationException("There are not enough tabs to switch back to the original tab.");
            }
        }

        public void NavigateToPage(string urlPath)
        {
            this.driver.Navigate().GoToUrl(urlPath);
        }

        public string GetPageTitle()
        {
            if (string.IsNullOrEmpty(driver.Title))
            {
                TakeScreenshot();
                throw new Exception("The title is null or empty");
            }
            return this.driver.Title;
        }

        public bool IsOpen(string urlPath)
        {
            string currentUrl = driver.Url;

            if (currentUrl != driver.Url)
            {
                throw new Exception($"Current URL '{currentUrl}' does not match the expected URL '{urlPath}'");
            }
            else
            {
                return currentUrl == urlPath;
            }

        }

        public bool IsOpen()
        {
            return driver.Url switch
            {
                var url when url == this.pageUrlTesting => true,
                var url when url == this.pageUrlStaging => true,
                var url when url == this.pageUrlLive => true,
                _ => throw new Exception("The current URL does not match any expected URL."),
            };
        }

        public void RefreshPage()
        {
            this.driver.Navigate().Refresh();
        }

        public void ReturnBack()
        {
            this.driver.Navigate().Back();
        }

        public void MoveForward()
        {
            this.driver.Navigate().Forward();
        }

        public string GetCurrentUrl()
        {
            return this.driver.Url;
        }

        public void DeleteAllCookies()
        {
            this.driver.Manage().Cookies.DeleteAllCookies();
        }

        public string GetText(IWebElement element)
        {
            return element.Text;
        }

        public void TakeScreenshot()
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string dateTime = initDateTimeFormat.ToString().Replace(':', '.');
            screenshot
                .SaveAsFile($@"C:\Users\TomislavIliev\Desktop\Screenshot by Selenium\{dateTime}.png");
        }
        public void HighlightElement(IWebElement element)
        {
            var jsDriver = (IJavaScriptExecutor)driver;
            jsDriver.ExecuteScript("arguments[0].style.border='3px solid red'", element);
        }

        public void ClickOnButton(IWebElement element)
        {
            WaitUntilElementIsClickable(element);
            element.Click();
        }

        public void HoverOverElement(IWebElement element)
        {
            action = new Actions(driver);
            WaitUntilElementIsClickable(element);
            action.MoveToElement(element).Perform();
        }


        public void jsClick(IWebElement elementToClick)
        {
            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
            ex.ExecuteScript("arguments[0].click();", elementToClick);
        }



    }
}
