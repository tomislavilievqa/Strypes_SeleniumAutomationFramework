using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strypes_SeleniumAutomationFramework.Properties
{
    public class ElementCoordinates
    {
        public int GetElementXCoordinate(IWebElement element)
        {
            Point location = element.Location;
            return location.X;
        }

        public int GetElementYCoordinate(IWebElement element)
        {
            Point location = element.Location;
            return location.Y;
        }
    }
}
