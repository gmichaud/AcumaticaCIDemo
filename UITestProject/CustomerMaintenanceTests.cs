using Microsoft.VisualStudio.TestTools.UnitTesting;
using UITestProject.Base;
using UITestProject.Extensions;

namespace UITestProject
{
    [TestClass]
    public class CustomerMaintenanceTests : UITestBase
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