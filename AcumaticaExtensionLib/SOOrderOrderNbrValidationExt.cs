using PX.Data;
using PX.Objects.SO;
using System;

namespace AcumaticaExtensionLib
{
    public class SOOrderOrderNbrValidationExt : PXGraphExtension<SOOrderEntry>
    {
        private const decimal OrderNbrRequiredAmount = 1000;

        public void _(Events.RowUpdated<SOOrder> e)
        {
            var order = (SOOrder) e.Row;

            if (order.CuryOrderTotal >= OrderNbrRequiredAmount && String.IsNullOrWhiteSpace(order.CustomerOrderNbr))
            {
                PXUIFieldAttribute.SetWarning<SOOrder.customerOrderNbr>(e.Cache, e.Row, 
                    $"Customer order number is required for orders of more than {OrderNbrRequiredAmount.ToString("C")}");
            }
            else
            {
                PXUIFieldAttribute.SetWarning<SOOrder.customerOrderNbr>(e.Cache, e.Row, null);
            }
        }
    }
}