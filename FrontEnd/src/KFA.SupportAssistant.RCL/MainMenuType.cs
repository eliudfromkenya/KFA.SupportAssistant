using System;

namespace KFA.SupportAssistant.RCL;

[Flags]
public enum MainMenuType : short
{
  None = 0,
  GeneralLedger = 2,
  Budgeting = 4,
  General = 8,
  Sales = 16,
  Purchases = 1,
  AccountReceivables = 32,
  System = 128,
  Payroll = 256,
  Projects = 512,
 // AboutUs = 1024,
  AboutUs = 2048,
  AccountPayables = 64
}
