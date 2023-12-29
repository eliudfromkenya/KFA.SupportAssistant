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
      SVGIcon = EmployeesSVGIcons.Employeelist
    },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "PaymentList",
        Text = "Dues Payments",
        URI = Constants.PaymentList,
        SVGIcon = EmployeesSVGIcons.PaymentList
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Employee Details",
        Text = "Staff Details",
        URI = Constants.EmployeeDetails,
        SVGIcon = EmployeesSVGIcons.EmployeeDetails
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Payment Detail",
        Text = "Payment Detail",
        URI = Constants.PaymentDetails,
        SVGIcon = EmployeesSVGIcons.PaymentDetails
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Employee Statement",
        Text = "Staff Statement",
        URI = Constants.Statement,
        SVGIcon = EmployeesSVGIcons.Statement
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Payments Detail Report",
        Text = "Payments Detail Report",
        URI = Constants.PaymentsDetailReport,
        SVGIcon = EmployeesSVGIcons.PaymentsDetailReport
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Employee Detail Details",
        Text = "Staff Details",
        URI = Constants.EmployeeDetailReport,
        SVGIcon = EmployeesSVGIcons.EmployeeDetailReport
      },
      new GeneralMenuItem
      {
        Group = "Employees",
        IsVisible = true,
        MainMenuType = MainMenuType.Payroll,
        Name = "Payment Summaries",
        Text = "Payment Summaries",
        URI = Constants.PaymentsSummaries,
        SVGIcon = EmployeesSVGIcons.PaymentsSummaries
      }
    ];
  }
}
