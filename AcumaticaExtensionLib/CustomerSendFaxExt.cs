using PX.Data;
using PX.Objects.AR;

namespace AcumaticaExtensionLib
{
    public class CustomerSendFaxExt : PXGraphExtension<CustomerMaint>
    {
        public PXAction<PX.Objects.AR.Customer> SendFax;

        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Send Fax")]
        protected void sendFax()
        {
            var defContactAddress = Base.GetExtension<CustomerMaint.DefContactAddressExt>();
            defContactAddress.DefContact.Current.Fax = "Who still uses a Fax?";
            defContactAddress.DefContact.Update(defContactAddress.DefContact.Current);
        }
    }
}