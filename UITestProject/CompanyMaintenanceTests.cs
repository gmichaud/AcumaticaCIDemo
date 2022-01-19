using Microsoft.VisualStudio.TestTools.UnitTesting;
using UITestProject.Base;
using UITestProject.Extensions;

namespace UITestProject
{
    [TestClass]
    public class CompanyMaintenanceTests : UITestBase
    {
        [TestMethod]
        public void TestThatFirstCompanyIsCapital()
        {
            var company = new Company();
            company.OpenScreen();
            company.First();
            Assert.AreEqual("CAPITAL", company.Summary.AcctCD.GetValue());
        }

        [TestMethod]
        public void TestThatFirstCompanyIsCapitalASecondTime()
        {
            var company = new Company();
            company.OpenScreen();
            company.First();
            Assert.AreEqual("CAPITAL", company.Summary.AcctCD.GetValue());
        }
    }
}