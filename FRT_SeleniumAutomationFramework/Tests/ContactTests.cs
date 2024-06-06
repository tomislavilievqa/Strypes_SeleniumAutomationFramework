using AngleSharp.Dom;
using AventStack.ExtentReports;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Strypes_SeleniumAutomationFramework.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strypes_SeleniumAutomationFramework.Tests
{
    [TestFixture("Chrome", "123.0", "Windows 11", "Chrome123")]
    [TestFixture("Firefox", "124.0", "Windows 11", "Firefox124")]
    public class ContactTests : BaseTest
    {
        private ContactPage Contact => new ContactPage(Driver, this.environment);
        private Homepage Homepage => new Homepage(Driver, this.environment);
        private NavigationBar NavigationBar => new NavigationBar(Driver, this.environment);

        public const string emailFieldErrorMessageLocator = "//*[@id='hsForm_9246f6ce-b893-4249-8362-96d17c0861f5']/fieldset[2]/div/ul/li/label";
        public const string privacyPolicyCheckboxErrorMessageLocator = "//*[@id=\"hsForm_9246f6ce-b893-4249-8362-96d17c0861f5\"]/fieldset[5]/div/div[1]/div/div/ul/li/label";
        public const string subscriptionCheckboxErrorMessageLocator = "//*[@id=\"hsForm_9246f6ce-b893-4249-8362-96d17c0861f5\"]/div[1]/ul/li/label";
        public ContactTests(string browser, string version, string os, string name) : base(browser, version, os, name)
        {
        }

        [Test]
        [Category("Smoke")]
        [Description("Test to ensure the 'Contact' page could be reached")]
        [Author("Tomislav Iliev")]
        public void VerifyThatCareersIsReachable()
        {

            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

            NavigationBar.ContactUsButton.Click();
            Assert.That(Contact.IsOpen());

        }

        [Test]
        [Category("Functional")]
        [Description("This test verifies the successful submission of the contact form with all required fields filled correctly.")]
        [Author("Tomislav Iliev")]
        public void VerifyTheSuccessfulFormSubmission()
        {
            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

            NavigationBar.ContactUsButton.Click();
            Assert.That(Contact.IsOpen());

            Contact.FirstNameField.SendKeys("Tomislav - QA Automation Task");
            Contact.LastNameField.SendKeys("Iliev - QA Automation Task");
            Contact.EmailField.SendKeys("ilievtomislav@gmail.com");
            Contact.CompanyNameField.SendKeys("Strypes - QA Automation Task");
            Contact.MessageField.SendKeys("Note that the sole reason this message was sent was for testing. I am developing an Automation Testing Framework for Strypes, as a task for an interview!");
            Contact.PrivacyAndPolicyCheckbox.Click();
            Contact.SubscriptionCheckbox.Click();

            Contact.SendButton.Click();

            Assert.That(Contact.ThankYouMessage.Displayed);
        }

        [Test]
        [Category("Functional")]
        [Description("This test verifies the behavior of the contact form submission when mandatory fields are not filled.")]
        [Author("Tomislav Iliev")]
        public void CheckFormSubmissionWithUnfilledMandatoryFields()
        {

            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

            NavigationBar.ContactUsButton.Click();
            Assert.That(Contact.IsOpen());

            Contact.FirstNameField.SendKeys("Tomislav - QA Automation Task");
            Contact.LastNameField.SendKeys("Iliev - QA Automation Task");
            Contact.CompanyNameField.SendKeys("Strypes - QA Automation Task");
            Contact.MessageField.SendKeys("Note that the sole reason this message was sent was for testing. I am developing an Automation Testing Framework for Strypes, as a task for an interview!");

            Contact.SendButton.Click();

            Assert.That(Contact.EmailFieldErrorMessage.Displayed);
            Assert.That(Contact.EmailFieldErrorMessage.Text, Is.EqualTo("Please complete this required field."));
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Displayed);
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Text, Is.EqualTo("Please complete all required fields."));
            Assert.That(Contact.PrivacyPolicyCheckboxErrorMessage.Displayed);
            Assert.That(Contact.PrivacyPolicyCheckboxErrorMessage.Text, Is.EqualTo("Please complete this required field."));
        }


        [Test]
        [Category("Functional")]
        [Description("This test verifies the behavior of the contact form submission when no fields are filled.")]
        [Author("Tomislav Iliev")]
        public void CheckFormSubmissionWithoutFillingAnyField()
        {

            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

            NavigationBar.ContactUsButton.Click();
            Assert.That(Contact.IsOpen());

            Contact.SendButton.Click();

            Assert.That(Contact.EmailFieldErrorMessage.Displayed);
            Assert.That(Contact.EmailFieldErrorMessage.Text, Is.EqualTo("Please complete this required field."));
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Displayed);
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Text, Is.EqualTo("Please complete all required fields."));
            Assert.That(Contact.PrivacyPolicyCheckboxErrorMessage.Displayed);
            Assert.That(Contact.PrivacyPolicyCheckboxErrorMessage.Text, Is.EqualTo("Please complete this required field."));

        }


        [Test]
        [Category("Functional")]
        [Description("This test verifies the behavior of the contact form submission with various invalid email addresses.")]
        [Author("Tomislav Iliev")]
        public void CheckFormSubmissionWithInvalidEmailAddress()
        {


            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

            NavigationBar.ContactUsButton.Click();
            Assert.That(Contact.IsOpen());

            Contact.FirstNameField.SendKeys("Tomislav - QA Automation Task");
            Contact.LastNameField.SendKeys("Iliev - QA Automation Task");
            Contact.CompanyNameField.SendKeys("Strypes - QA Automation Task");
            Contact.MessageField.SendKeys("Note that the sole reason this message was sent was for testing. I am developing an Automation Testing Framework for Strypes, as a task for an interview!");
            Contact.PrivacyAndPolicyCheckbox.Click();
            Contact.SubscriptionCheckbox.Click();

            // as I am not aware what are the currect validations on this field,
            //I have added some e-mails that should be wrong based on the most recent and popular standarts

            var wrongEmails = new List<string>()
            {
                "tomislav.iliev",
                "tomislav.iliev@",
                "tomislav.iliev@gmail",
                "tomislav.iliev@gmail.c",
                "tomislav.iliev@@gmail.com",

            };

            for (int i = 0; i < wrongEmails.Count; i++)
            {
                Contact.EmailField.SendKeys(wrongEmails[i]);
                Contact.SendButton.Click();
                Assert.That(Contact.EmailFieldErrorMessage.Text, Is.EqualTo("Email must be formatted correctly."));
                Contact.EmailField.Clear();
                Assert.That(Contact.EmailFieldErrorMessage.Text, Is.EqualTo("Please complete this required field."));
            }

            Assert.That(Contact.IsElementDisplayed(By.XPath(subscriptionCheckboxErrorMessageLocator)), Is.False, "Subscription error message is not present.");
            Assert.That(Contact.IsElementDisplayed(By.XPath(privacyPolicyCheckboxErrorMessageLocator)), Is.False, "Privacy Policy error message is not present.");


        }


        [Test]
        [Category("Functional")]
        [Description("This test verifies the behavior of the contact form submission with an empty email address field.")]
        [Author("Tomislav Iliev")]
        public void CheckFormSubmissionWithEmptyEmailAddress()
        {

            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

            NavigationBar.ContactUsButton.Click();
            Assert.That(Contact.IsOpen());

            Contact.FirstNameField.SendKeys("Tomislav - QA Automation Task");
            Contact.LastNameField.SendKeys("Iliev - QA Automation Task");
            Contact.CompanyNameField.SendKeys("Strypes - QA Automation Task");
            Contact.MessageField.SendKeys("Note that the sole reason this message was sent was for testing. I am developing an Automation Testing Framework for Strypes, as a task for an interview!");
            Contact.PrivacyAndPolicyCheckbox.Click();
            Contact.SubscriptionCheckbox.Click();

            Contact.SendButton.Click();

            Assert.That(Contact.EmailFieldErrorMessage.Displayed);
            Assert.That(Contact.EmailFieldErrorMessage.Text, Is.EqualTo("Please complete this required field."));
            Assert.That(Contact.IsElementDisplayed(By.XPath(privacyPolicyCheckboxErrorMessageLocator)), Is.False, "Privacy Policy error message is not present.");
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Displayed);
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Text, Is.EqualTo("Please complete all required fields."));

        }

        [Test]
        [Category("Functional")]
        [Description("This test verifies the behavior of the contact form submission without accepting the Privacy Policy.")]
        [Author("Tomislav Iliev")]
        public void CheckFormSubmissionWithoutAcceptingPrivacyPolicy()
        {
            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

            NavigationBar.ContactUsButton.Click();
            Assert.That(Contact.IsOpen());

            Contact.FirstNameField.SendKeys("Tomislav - QA Automation Task");
            Contact.LastNameField.SendKeys("Iliev - QA Automation Task");
            Contact.EmailField.SendKeys("ilievtomislav@gmail.com");
            Contact.CompanyNameField.SendKeys("Strypes - QA Automation Task");
            Contact.MessageField.SendKeys("Note that the sole reason this message was sent was for testing. I am developing an Automation Testing Framework for Strypes, as a task for an interview!");
            Contact.SubscriptionCheckbox.Click();

            Contact.SendButton.Click();

            Assert.That(Contact.PrivacyPolicyCheckboxErrorMessage.Displayed);
            Assert.That(Contact.PrivacyPolicyCheckboxErrorMessage.Text, Is.EqualTo("Please complete this required field."));
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Displayed);
            Assert.That(Contact.SubscriptionCheckboxErrorMessage.Text, Is.EqualTo("Please complete all required fields."));
            Assert.That(Contact.IsElementDisplayed(By.XPath(emailFieldErrorMessageLocator)), Is.False, "Email error message is not present.");

        }





    }
}
