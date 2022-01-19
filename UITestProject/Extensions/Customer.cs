using Controls.ToolBarButton;
using GeneratedWrappers.Acumatica;

namespace UITestProject.Extensions
{
    public class Customer : AR303000_CustomerMaint
    {
        public c_baccount_baccount Summary => BAccount_BAccount;

        public c_defcontact_defcontact1 DefaultContact => DefContact_DefContact1;

        public virtual void SendFax()
        {
            var btn = new ToolBarButton("css=#ctl00_phDS_ds_ToolBar_sendFax,#ctl00_phDS_ds_ToolBar_sendFax_item", "Send Fax", null, ToolBar.MenuOpener);
            btn.Click();
        }
    }
}
