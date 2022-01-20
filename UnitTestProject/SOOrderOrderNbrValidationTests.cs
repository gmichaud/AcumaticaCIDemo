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
    public class SOOrderOrderNbrValidationTests : UnitTestWithSOSetup
    {
		private SOOrderEntry _graph;
		private InventoryItem _stockItem;
		private Customer _customer;

		[TestInitialize]
		public void Initialize()
        {
			SetupOrganizationAndBranch<SOOrderEntry>();
			SetupSO<SOOrderEntry>();
			SetupSOOrderTypeAndTypeOperation<SOOrderEntry>();

			_graph = PXGraph.CreateInstance<SOOrderEntry>();
			
			//This will force initialization graph extension
			var ext = _graph.GetExtension<AcumaticaExtensionLib.SOOrderOrderNbrValidationExt>();

			SetupCommonTestData();
		}

		private void SetupCommonTestData()
        {
			InsertINUnit(_graph, "EA");

			_stockItem = (InventoryItem)_graph.Caches[typeof(InventoryItem)].Insert(
				 new InventoryItem
				 {
					 InventoryCD = "TESTITEM",
					 BaseUnit = "EA",
				 });

			_graph.Caches[typeof(INUnit)].Insert(
				new INUnit
				{
					UnitType = INUnitType.InventoryItem,
					InventoryID = _stockItem.InventoryID,
					FromUnit = "EA",
					ToUnit = "EA",
					UnitMultDiv = "D",
					UnitRate = 1
				});

			_customer = InsertCustomer(_graph, "TestCustomer");
		}

		[TestMethod]
        public void OrderOfMoreThan1000RequiresOrderNbr()
        {	
			// Execute Action
			SOOrder order = _graph.Document.Insert(
				new SOOrder()
				{
					OrderType = "SO",
					OrderNbr = "1234",
					CustomerID = _customer.BAccountID,
					CustomerLocationID = _customer.DefLocationID,
					BranchID = -1,
				});

			SOLine orderLine = _graph.Transactions.Insert(
				new SOLine()
				{
					InventoryID = _stockItem.InventoryID,
					Qty = 1,
					CuryUnitPrice = 3000
				});

			var warning = PXUIFieldAttribute.GetWarning<SOOrder.customerOrderNbr>(_graph.Document.Cache, order);
			Assert.AreEqual("Customer order number is required for orders of more than $1,000.00", warning);

			//Set an order number, and check that the requirement is fulfilled.
			order.CustomerOrderNbr = "ABC123";
			_graph.Document.Update(order);

			warning = PXUIFieldAttribute.GetWarning<SOOrder.customerOrderNbr>(_graph.Document.Cache, order);
			Assert.IsNull(warning);
		}

		[TestMethod]
		public void OrderOfLessThan1000DoesNotRequireOrderNbr()
		{
			// Execute Action
			SOOrder order = _graph.Document.Insert(
				new SOOrder()
				{
					OrderType = "SO",
					OrderNbr = "1234",
					CustomerID = _customer.BAccountID,
					CustomerLocationID = _customer.DefLocationID,
					BranchID = -1,
				});

			SOLine orderLine = _graph.Transactions.Insert(
				new SOLine()
				{
					InventoryID = _stockItem.InventoryID,
					Qty = 1,
					CuryUnitPrice = 950
				});

			var warning = PXUIFieldAttribute.GetWarning<SOOrder.customerOrderNbr>(_graph.Document.Cache, order);
			Assert.IsNull(warning);
		}
	}
}
