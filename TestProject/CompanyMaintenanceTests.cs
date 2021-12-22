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
    public class CompanyMaintenanceTests : AcumaticaUITestBase
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