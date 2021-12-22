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
    public class CustomerMaintenanceTests : AcumaticaUITestBase
    {
        [TestMethod]
        public void TestSendFaxButton()
        {
            var customer = new Customer();
            customer.OpenScreen();
            customer.First();
            customer.SendFax();
            Assert.AreEqual("Who still uses a Fax?", customer.DefaultContact.Fax.GetValue());
        }
    }
}