using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SupportAssistant.RCL.Models;

namespace KFA.SupportAssistant.RCL.Pages.Employees;

public partial class Index : IMainPage
{
  public GeneralMenuItem[]? MenuItems { get; set; }

  public Index()
  {
    MenuItems = [new GeneralMenuItem
    {
      Group = "Employees",
      IsVisible = true,
      MainMenuType = MainMenuType.Payroll,
      Name = "EmployeeList",
      Text = "Staff On Dues",
      URI = Constants.Employeelist,
      SVGIcon = SVGIcons.Employeelist
    },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "PaymentList",
        Text = "Dues Payments",
        URI = Constants.PaymentList,
        SVGIcon = SVGIcons.PaymentList
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Employee Details",
        Text = "Staff Details",
        URI = Constants.EmployeeDetails,
        SVGIcon = SVGIcons.EmployeeDetails
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Payment Detail",
        Text = "Payment Detail",
        URI = Constants.PaymentDetails,
        SVGIcon = SVGIcons.PaymentDetails
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Employee Statement",
        Text = "Staff Statement",
        URI = Constants.Statement,
        SVGIcon = SVGIcons.Statement
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Payments Detail Report",
        Text = "Payments Detail Report",
        URI = Constants.PaymentsDetailReport,
        SVGIcon = SVGIcons.PaymentsDetailReport
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Employee Detail Details",
        Text = "Staff Details",
        URI = Constants.EmployeeDetailReport,
        SVGIcon = SVGIcons.EmployeeDetailReport
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Payment Summaries",
        Text = "Payment Summaries",
        URI = Constants.PaymentsSummaries,
        SVGIcon = SVGIcons.PaymentsSummaries
      }
    ];
  }
}
