using Microsoft.VisualStudio.TestTools.UnitTesting;
using PX.Data;
using PX.Tests.Unit;
using PX.Objects.SO;
using PX.Objects.IN;
using UnitTestProject.Setup;
using PX.Objects.AR;

namespace UnitTestProject
{
    [TestClass]
    public class SOOrderEntryTests : UnitTestWithSOSetup
    {
        [TestMethod]
        public void TestMethod1()
        {
			SetupOrganizationAndBranch<SOOrderEntry>();
			SetupSO<SOOrderEntry>();
            SetupSOOrderTypeAndTypeOperation<SOOrderEntry>();

            var graph = PXGraph.CreateInstance<SOOrderEntry>();
            var ext = graph.GetExtension<AcumaticaExtensionLib.SOOrderEntryExt>();

			InsertINUnit(graph, "EA");

			var stockItem = (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(
				 new InventoryItem
				 {
					 InventoryCD = "TESTITEM",
					 BaseUnit = "EA",
				 });

			graph.Caches[typeof(INUnit)].Insert(
				new INUnit
				{
					UnitType = INUnitType.InventoryItem,
					InventoryID = stockItem.InventoryID,
					FromUnit = "EA",
					ToUnit = "EA",
					UnitMultDiv = "D",
					UnitRate = 1
				});

			Customer customer = InsertCustomer(graph, "TestCustomer");
			
			// Execute Action
			SOOrder order = graph.Document.Insert(
				new SOOrder()
				{
					OrderType = "SO",
					OrderNbr = "1234",
					CustomerID = customer.BAccountID,
					CustomerLocationID = customer.DefLocationID,
					BranchID = -1,
				});

			SOLine orderLine = graph.Transactions.Insert(
				new SOLine()
				{
					InventoryID = stockItem.InventoryID,
					Qty = 1,
					CuryUnitPrice = 100
				});

			//graph.Actions.PressSave();
		}
    }
}
