using PX.Data;
using PX.Objects.SO;
using System;

namespace AcumaticaExtensionLib
{
    public class SOOrderEntryExt : PXGraphExtension<SOOrderEntry>
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public void BAccountR_RowInserted(PXCache sender, PXRowInsertedEventArgs e)
        {

        }

        [PXOverride]
        public void Persist(Action baseMethod)
        {
            if (Base.Document.Current.CuryOrderTotal >= 1000 && String.IsNullOrWhiteSpace(Base.Document.Current.CustomerOrderNbr))
            { 
                throw new PXException("Customer order number is required for orders of more than $1,000");
            }

            baseMethod();
        }
    }
}