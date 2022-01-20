using Microsoft.VisualStudio.TestTools.UnitTesting;
using PX.Data;
using PX.Objects.AR;
using UnitTestProject.Setup;

namespace UnitTestProject
{
    [TestClass]
    public class CustomerSendFaxTests : UnitTestWithARSetup
    {
        [TestInitialize]
        public void Initialize()
        {
            SetupAR<CustomerMaint>();
            SetupOrganizationAndBranch<CustomerMaint>();
        }

        [TestMethod]
        public void TestSendFaxButton()
        {
            //Arrange
            var graph = PXGraph.CreateInstance<CustomerMaint>();
            var faxExt = graph.GetExtension<AcumaticaExtensionLib.CustomerSendFaxExt>();

            Customer customer = graph.BAccount.Insert(
                new Customer()
                {
                    AcctCD = "TEST",
                    AcctName = "Test"
                });

            //Act
            faxExt.SendFax.Press();

            //Assert
            var contactExt = graph.GetExtension<CustomerMaint.DefContactAddressExt>();
            Assert.AreEqual("Who still uses a Fax?", contactExt.DefContact.Current.Fax);
        }
    }
}