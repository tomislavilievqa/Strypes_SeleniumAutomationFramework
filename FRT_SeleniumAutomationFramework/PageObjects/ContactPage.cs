using Strypes_SeleniumAutomationFramework.Properties;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strypes_SeleniumAutomationFramework.PageObjects
{
    public class ContactPage : BasePage
    {
        public override string pageTitleTesting => "Testing Contact - Strypes";
        public override string pageTitleStaging => "Staging Contact - Strypes";
        public override string pageTitleLive => "Contact - Strypes";
        public override string pageUrlTesting => $"https://{environment.BaseUrl}/contact/";
        public override string pageUrlStaging => $"https://{environment.BaseUrl}/contact/";
        public override string pageUrlLive => $"https://{environment.BaseUrl}/contact/";

      

        public ContactPage(IWebDriver driver, CurrentEnvironment environment) : base(driver, environment)
        {

        }

        public IWebElement FirstNameField => WaitAndFindElement(By.Id("firstname-9246f6ce-b893-4249-8362-96d17c0861f5"));
        public IWebElement LastNameField => WaitAndFindElement(By.Id("lastname-9246f6ce-b893-4249-8362-96d17c0861f5"));
        public IWebElement EmailField => WaitAndFindElement(By.Id("email-9246f6ce-b893-4249-8362-96d17c0861f5"));
        public IWebElement CompanyNameField => WaitAndFindElement(By.Id("company-9246f6ce-b893-4249-8362-96d17c0861f5"));
        public IWebElement MessageField => WaitAndFindElement(By.Id("message-9246f6ce-b893-4249-8362-96d17c0861f5"));
        public IWebElement PrivacyAndPolicyCheckbox => WaitAndFindElement(By.Id("LEGAL_CONSENT.subscription_type_4681882-9246f6ce-b893-4249-8362-96d17c0861f5"));
        public IWebElement SubscriptionCheckbox => WaitAndFindElement(By.Id("LEGAL_CONSENT.subscription_type_4673944-9246f6ce-b893-4249-8362-96d17c0861f5"));
        public IWebElement SendButton => WaitAndFindElement(By.XPath("//input[@type='submit']"));
        public IWebElement EmailFieldErrorMessage => WaitAndFindElement(By.XPath("//*[@id=\"hsForm_9246f6ce-b893-4249-8362-96d17c0861f5\"]/fieldset[2]/div/ul/li/label"));
        public IWebElement PrivacyPolicyCheckboxErrorMessage => WaitAndFindElement(By.XPath("//*[@id=\"hsForm_9246f6ce-b893-4249-8362-96d17c0861f5\"]/fieldset[5]/div/div[1]/div/div/ul/li/label"));
        public IWebElement SubscriptionCheckboxErrorMessage => WaitAndFindElement(By.XPath("//*[@id=\"hsForm_9246f6ce-b893-4249-8362-96d17c0861f5\"]/div[1]/ul/li/label"));
        public IWebElement ThankYouMessage => WaitAndFindElement(By.XPath("//div[contains(text(), 'Thank you for submitting the form.')]"));


        public const string emailFieldErrorMessageLocator = "//*[@id=\"hsForm_9246f6ce-b893-4249-8362-96d17c0861f5\"]/fieldset[2]/div/ul/li/label";
        public const string privacyPolicyCheckboxErrorMessageLocator = "//*[@id='hsForm_9246f6ce-b893-4249-8362-96d17c0861f5']/fieldset[2]/div/ul/li/label";
        public const string subscriptionCheckboxErrorMessageLocator = "//*[@id='hsForm_9246f6ce-b893-4249-8362-96d17c0861f5']/fieldset[2]/div/ul/li/label";
    }
}
