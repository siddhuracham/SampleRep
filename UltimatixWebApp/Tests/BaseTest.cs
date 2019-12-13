using Logger;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace UltimatixWebApp.Tests
{
    public class BaseTest
    {
        IWebDriver SetUpDriverInstance(string browserType)
        {
            switch (browserType)
            {
                case "Firefox":
                    //new instance of browser profile var profile = new FirefoxProfile(); //retrieving settings from config file var firefoxSettings = ConfigurationManager.GetSection("FirefoxSettings") as NameValueCollection; //if there are any settings
                    var profile = new FirefoxProfile();
                    var firefoxSettings = ConfigurationManager.GetSection("FirefoxSettings");
                    if (firefoxSettings != null)
                        //loop through all of them
                        for (var i = 0; i < firefoxSettings.Count; i++) //and verify all of them
                            switch (firefoxSettings[i])
                            { //if current settings value is "true"
                                case "true":
                                    profile.SetPreference(firefoxSettings.GetKey(i), true);
                                    break;
                                //if "false"
                                case "false":
                                    profile.SetPreference(firefoxSettings.GetKey(i), false);
                                    break;
                                //otherwise
                                default:
                                    int temp;
                                    //an attempt to parse current settings value to an integer. Method TryParse returns True if the attempt is successful (the string is integer) or return False (if the string is just a string and cannot be cast to a number) if (Int32.TryParse(firefoxSettings.Get(i), out temp)) profile.SetPreference(firefoxSettings.GetKey(i), temp); else
                                    profile.SetPreference(firefoxSettings.GetKey(i), firefoxSettings[i]);
                                    break;
                            }
                    return new FirefoxDriver(profile);
                case "Internet Explorer":
                    var IEoptions = new InternetExplorerOptions();
                    var ieSettings = ConfigurationManager.GetSection("IESettings") as NameValueCollection;
                    if (ieSettings != null)
                    {
                        IEoptions.IgnoreZoomLevel = ieSettings["IgnoreZoomLevel"].ToString(CultureInfo.InvariantCulture) == "true";
                        IEoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = ieSettings["IntroduceInstabilityByIgnoringProtectedModeSettings"] == "true";
                    }
                    return new InternetExplorerDriver(IEoptions);

                case "Chrome":
                    var options = new ChromeOptions();
                    var chromeSettings = ConfigurationManager.GetSection("ChromeSettings") as NameValueCollection;
                    var optionsList = new List<string>();
                    if (chromeSettings != null)
                        for (var i = 0; i < chromeSettings.Count; i++)
                            if (chromeSettings[i] == "true") optionsList.Add(chromeSettings.GetKey(i));
                    options.AddArguments(optionsList);
                    return new ChromeDriver(options);
                default:
                    Report.AddError(browserType + " web driver instance cannot be initialized. Test will by terminated. Verify configuration parameters.");
                    throw new Exception();
            }
        }
    }
}
