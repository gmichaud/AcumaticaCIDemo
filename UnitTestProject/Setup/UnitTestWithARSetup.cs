using System;
using System.Collections;
using System.Linq;

using Autofac;

using PX.Data;
using PX.Tests.Unit;
using PX.Objects.AP;
using PX.Objects.CM.Extensions;
using PX.Objects.GL;
using PX.Objects.GL.FinPeriods;
using PX.Objects.GL.DAC;
using PX.Objects.SO;
using PX.Objects.AR;
using PX.Objects.CR;

namespace UnitTestProject.Setup
{

	public abstract class UnitTestWithARSetup : UnitTestWithGLSetup
	{
		protected void SetupAR<Graph>()
		where Graph : PXGraph
		{
			SetupGL<Graph>();
			Setup<Graph>(
				new ARSetup
				{
					RequireControlTotal = false
				});
		}
		public virtual Customer InsertCustomer(PXGraph graph, string customerCD)
		{
			var customer = Insert<Customer>(graph, new Customer
			{
				AcctCD = customerCD,
				AcctName = customerCD,
				CuryID = "USD",
				Type = BAccountType.CustomerType,
				Status = CustomerStatus.Active,
			});
			var location = Insert<Location>(graph, new Location()
			{
				BAccountID = customer.BAccountID,
				LocationCD = "MockLocaltion",
				IsActive = true,
				LocType = LocTypeList.CustomerLoc
			});
			customer.DefLocationID = location.LocationID;

			return Update<Customer>(graph, customer);
		}
	}
}
