using AventStack.ExtentReports;
using Strypes_SeleniumAutomationFramework.Core;
using Strypes_SeleniumAutomationFramework.PageObjects;
using Halforbit.ObjectTools.ObjectBuild.Implementation;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strypes_SeleniumAutomationFramework.Tests
{
    [TestFixture("Chrome", "123.0", "Windows 11", "Chrome123")]
    [TestFixture("Firefox", "124.0", "Windows 11", "Firefox124")]
    public class HomepageTests : BaseTest
    {
        public HomepageTests(string browser, string version, string os, string name) : base(browser, version, os, name)
        {
        }

        private Homepage Homepage => new Homepage(Driver, environment);

        [Test]
        [Category("Smoke")]
        [Description("Test to ensure the 'Homepage' could be reached")]
        [Author("Tomislav Iliev")]

        public void Test_Open_The_Homepage()
        {

            Homepage.NavigateToPage(_baseUrl);
            Assert.That(Homepage.IsOpen(_baseUrl));

        }
    }
}
