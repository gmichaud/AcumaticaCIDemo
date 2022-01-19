using GeneratedWrappers.Acumatica;

namespace UITestProject.Extensions
{
    public class Company : CS101500_OrganizationMaint
    {
        public c_baccount_pxformview1 Summary => BAccount_PXFormView1;
        public c_defaddress_defaddress DefaultAddress => DefAddress_DefAddress;
        public c_organizationview_pxformview1 OrganizationSettings => OrganizationView_PXFormView1;
        public c_organizationview_company BaseCurrency => OrganizationView_Company;
        public c_commonsetup_commonsettings CommonSettings => Commonsetup_commonsettings;
        public c_organizationledgerlinkwithledgerselect_grdledgerlinks Ledgers => OrganizationLedgerLinkWithLedgerSelect_grdLedgerLinks;
        public c_branchesview_grdbranches Branches => BranchesView_grdBranches;
        public c_employees_grdemployees Employees => Employees_grdEmployees;
        public c_organizationview_configurationsettings ConfigurationSettings => OrganizationView_ConfigurationSettings;
    }
}
