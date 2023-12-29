using KFA.SupportAssistant.RCL.Models;

namespace KFA.SupportAssistant.RCL.Data;
public static class SideMenuItems
{
   public static SideMenuItem[] GetSideMenuItems() 
  {
    return
      [
        new SideMenuItem { SVGIcon = SVGIcons.Projects, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.Projects, Name = "Projects", Text = "Ms Dynamics", URI = "projects" },
        new SideMenuItem { SVGIcon = SVGIcons.Payroll, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.Payroll, Name = "Payroll", Text = "Employees", URI = "employees" },
        new SideMenuItem { SVGIcon = SVGIcons.Budgeting, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.Budgeting, Name = "Budgeting", Text = "Budgeting", URI = "budgeting" },
        new SideMenuItem { SVGIcon = SVGIcons.General, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.General, Name = "General", Text = "General", URI = "general" },
        new SideMenuItem { SVGIcon = SVGIcons.Purchases, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.Purchases, Name = "Purchases", Text = "Purchases", URI = "purchases" },
        new SideMenuItem { SVGIcon = SVGIcons.Sales, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.Sales, Name = "Sales", Text = "Sales", URI = "sales" },
        new SideMenuItem { SVGIcon = SVGIcons.GeneralLedger, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.GeneralLedger, Name = "GeneralLedger", Text = "General Ledger", URI = "general-ledgers" },
        new SideMenuItem { SVGIcon = SVGIcons.AccountReceivables, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.AccountReceivables, Name = "AccountReceivables", Text = "Account Receivables", URI = "account-receivables" },
        new SideMenuItem { SVGIcon = SVGIcons.AccountPayables, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.AccountPayables, Name = "AccountPayables", Text = "Account Payables", URI = "account-payables" },
        new SideMenuItem { SVGIcon = SVGIcons.System, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.System, Name = "System", Text = "System", URI = "system" },
        new SideMenuItem { SVGIcon = SVGIcons.AboutUs, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.AboutUs, Name = "AboutUs", Text = "About Us", URI = "about-us" },
        new SideMenuItem { SVGIcon = SVGIcons.None, Group = "Main", IsVisible = true, MainMenuType = MainMenuType.None, Name = "None", Text = "None", URI = "none" },
      ];
  } 
}
