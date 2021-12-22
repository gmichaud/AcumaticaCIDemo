using Core;
using Core.Config;
using Core.Login;
using Core.TestExecution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using TestProject.Extensions;

namespace TestProject
{
    [TestClass]
    public class AcumaticaUITestBase
    {
        [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void Initialize(TestContext context)
        {
            // This configuration would usually be loaded from a XML file when calling Config.ReadConfig()
            // but we can actually set most of the properties directly.
            // NOTE: Should be loaded from configuration and not hardcoded!
            Config.BrowserBin = @"C:\dev\TestSDK\TestSDK_21_205_0063_65\Chrome\chrome.exe";
            Config.SITE_DST_URL = @"https://summit.velixo.com/";
            Config.SITE_DST_LOGIN = "admin";
            Config.SITE_DST_PASSWORD = "testvelixo";

            // Config.SelectedBrowser cannot be changed since the property set is private
            // reflection will do the trick!
            typeof(Config).GetProperty("SelectedBrowser")?.SetValue(null, PX.QA.Tools.Browsers.Chrome);

            PxLogin.LoginToDestinationSite();
        }

        [ClassCleanup(InheritanceBehavior.BeforeEachDerivedClass)]
        public static void Cleanup()
        {
            // Internal method -- takes care of killing web driver
            // Support.KillProcessTree(Process.GetCurrentProcess().Id, null, null);
            MethodInfo? dynMethod = typeof(Support).GetMethod("KillProcessTree", BindingFlags.NonPublic | BindingFlags.Static);
            dynMethod?.Invoke(null, new object?[] { Process.GetCurrentProcess().Id, null, null });
        }
    }
}