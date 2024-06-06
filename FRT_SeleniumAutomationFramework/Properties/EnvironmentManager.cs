using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Strypes_SeleniumAutomationFramework.Properties
{
    public class EnvironmentManager
    {

        private CurrentEnvironment currentEnvironment;
        private readonly IConfigurationRoot configuration; // This field is used to store the configuration settings for the application.
        //private Dictionary<string, string> regions;
        private string pathToPredictionModel => Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\")); // This line defines a property pathToPredictionModel that calculates the full path to the prediction model directory. It uses Path.GetFullPath and Path.Combine methods to construct the path based on the current application's base directory.

        //add meaningful validations

        public EnvironmentManager(string defaultEnvironment = "Testing")//, string defaultRegion = "US" ) 
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(pathToPredictionModel)
            .AddJsonFile("environmentsConfig.json", optional: true, reloadOnChange: true);

            this.configuration = builder.Build();
            // this.regions = configuration.GetSection("Regions").Get<Dictionary<string, string>>();
            this.SetEnvironment(defaultEnvironment);//, defaultRegion); 
        }

        public void SetEnvironment(string environment)//, string defaultRegion = "US") This line declares a public method named SetEnvironment, which sets the current environment based on the provided environment name.
        {
            this.currentEnvironment = this.configuration.GetSection(environment).Get<CurrentEnvironment>(); //This line retrieves the configuration section corresponding to the provided environment name and maps it to an instance of CurrentEnvironment. It then assigns this instance to the currentEnvironment field.
            //this.currentEnvironment.Username = this.configuration[$"{environment}:Username"];
            //this.currentEnvironment.Password = this.configuration[$"{environment}:Password"];
            //this.currentEnvironment.BaseUrl += regions[defaultRegion];

        }

        public CurrentEnvironment GetCurrentEnvironment()
        {
            return this.currentEnvironment; //returns the current environment stored in the currentEnvironment field.
        }
    }
}
