using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using PX.Objects.IN;
using PX.Objects.CS;

namespace UnitTestProject.Setup
{
    public abstract class UnitTestWithSOSetup : UnitTestWithARSetup
    {
        protected void SetupSO<Graph>()
            where Graph : PXGraph
        {
            SetupAR<Graph>();
            Setup<Graph>(
                new SOSetup
                {
                });
        }
        public virtual void SetupSOOrderTypeAndTypeOperation<Graph>()
            where Graph : PXGraph
        {
            Setup<Graph>(
                new Numbering
                {
                    NumberingID = "SONUM",
                    Descr = "SO Numbering",
                    NewSymbol = "<NEW>",
                    UserNumbering = true
                },
                new SOOrderTypeOperation
                {
                    OrderType = "SO",
                    Operation = "I",
                },
                new SOOrderTypeT
                {
                    OrderType = "SO",
                    Behavior = "SO",
                },
                new SOOrderType
                {
                    OrderType = "SO",
                    Active = true,
                    DaysToKeep = 0,
                    Template = "SO",
                    IsSystem = true,
                    Behavior = "SO",
                    DefaultOperation = "I", 
                    //OrderNumberingID = "SONUM"
                }) ;
        }

        protected static void InsertINUnit(PXGraph graph, string unit)
        {
            graph.Caches[typeof(INUnit)].Insert(
               new INUnit
               {
                   UnitType = INUnitType.Global,
                   InventoryID = 0,
                   FromUnit = unit,
                   ToUnit = unit,
                   UnitMultDiv = "M",
                   UnitRate = 1m
               });
        }
    }
}
