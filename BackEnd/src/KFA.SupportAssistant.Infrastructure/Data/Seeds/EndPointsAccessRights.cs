using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Infrastructure.Data.Seeds;

internal static class EndPointsAccessRights
{
  public static async Task<bool> Process(AppDbContext db)
  {
    DefaultAccessRight[] groups = [

      #region Actual Budget Variances

      new DefaultAccessRight { Id = "ENP-101", Name = "Actual Budget Variances", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-102", Name = "Actual Budget Variances", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-103", Name = "Actual Budget Variances", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-104", Name = "Actual Budget Variances", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-105", Name = "Actual Budget Variances", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-106", Name = "Actual Budget Variances", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-107", Name = "Actual Budget Variances", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Actual Budget Variances

      #region Actual Budget Variances Batch Headers

      new DefaultAccessRight { Id = "ENP-111", Name = "Actual Budget Variances Batch Headers", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-112", Name = "Actual Budget Variances Batch Headers", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-113", Name = "Actual Budget Variances Batch Headers", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-114", Name = "Actual Budget Variances Batch Headers", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-115", Name = "Actual Budget Variances Batch Headers", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-116", Name = "Actual Budget Variances Batch Headers", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-117", Name = "Actual Budget Variances Batch Headers", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Actual Budget Variances Batch Headers

      #region Command Details

      new DefaultAccessRight { Id = "ENP-121", Name = "Command Details", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-122", Name = "Command Details", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-123", Name = "Command Details", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-124", Name = "Command Details", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-125", Name = "Command Details", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-126", Name = "Command Details", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-127", Name = "Command Details", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Command Details

      #region Communication Messages

      new DefaultAccessRight { Id = "ENP-131", Name = "Communication Messages", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-132", Name = "Communication Messages", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-133", Name = "Communication Messages", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-134", Name = "Communication Messages", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-135", Name = "Communication Messages", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-136", Name = "Communication Messages", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-137", Name = "Communication Messages", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Communication Messages

      #region Computer Anydesks

      new DefaultAccessRight { Id = "ENP-141", Name = "Computer Anydesks", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-142", Name = "Computer Anydesks", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-143", Name = "Computer Anydesks", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-144", Name = "Computer Anydesks", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-145", Name = "Computer Anydesks", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-146", Name = "Computer Anydesks", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-147", Name = "Computer Anydesks", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Computer Anydesks

      #region Cost Centres

      new DefaultAccessRight { Id = "ENP-151", Name = "Cost Centres", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-152", Name = "Cost Centres", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-153", Name = "Cost Centres", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-154", Name = "Cost Centres", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-155", Name = "Cost Centres", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-156", Name = "Cost Centres", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-157", Name = "Cost Centres", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Cost Centres

      #region Count Sheet Batches

      new DefaultAccessRight { Id = "ENP-161", Name = "Count Sheet Batches", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-162", Name = "Count Sheet Batches", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-163", Name = "Count Sheet Batches", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-164", Name = "Count Sheet Batches", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-165", Name = "Count Sheet Batches", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-166", Name = "Count Sheet Batches", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-167", Name = "Count Sheet Batches", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Count Sheet Batches

      #region Data Devices

      new DefaultAccessRight { Id = "ENP-171", Name = "Data Devices", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-172", Name = "Data Devices", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-173", Name = "Data Devices", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-174", Name = "Data Devices", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-175", Name = "Data Devices", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-176", Name = "Data Devices", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-177", Name = "Data Devices", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Data Devices

      #region Default Access Rights

      new DefaultAccessRight { Id = "ENP-181", Name = "Default Access Rights", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-182", Name = "Default Access Rights", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-183", Name = "Default Access Rights", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-184", Name = "Default Access Rights", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-185", Name = "Default Access Rights", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-186", Name = "Default Access Rights", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-187", Name = "Default Access Rights", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Default Access Rights

      #region Device Guids

      new DefaultAccessRight { Id = "ENP-191", Name = "Device Guids", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-192", Name = "Device Guids", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-193", Name = "Device Guids", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-194", Name = "Device Guids", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-195", Name = "Device Guids", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-196", Name = "Device Guids", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-197", Name = "Device Guids", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Device Guids

      #region Dues Payment Details

      new DefaultAccessRight { Id = "ENP-1A1", Name = "Dues Payment Details", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1A2", Name = "Dues Payment Details", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1A3", Name = "Dues Payment Details", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1A4", Name = "Dues Payment Details", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1A5", Name = "Dues Payment Details", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1A6", Name = "Dues Payment Details", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1A7", Name = "Dues Payment Details", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Dues Payment Details

      #region Employee Details

      new DefaultAccessRight { Id = "ENP-1B1", Name = "Employee Details", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1B2", Name = "Employee Details", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1B3", Name = "Employee Details", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1B4", Name = "Employee Details", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1B5", Name = "Employee Details", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1B6", Name = "Employee Details", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1B7", Name = "Employee Details", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Employee Details

      #region Expense Budget Batch Headers

      new DefaultAccessRight { Id = "ENP-1C1", Name = "Expense Budget Batch Headers", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1C2", Name = "Expense Budget Batch Headers", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1C3", Name = "Expense Budget Batch Headers", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1C4", Name = "Expense Budget Batch Headers", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1C5", Name = "Expense Budget Batch Headers", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1C6", Name = "Expense Budget Batch Headers", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1C7", Name = "Expense Budget Batch Headers", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Expense Budget Batch Headers

      #region Expenses Budget Details

      new DefaultAccessRight { Id = "ENP-1D1", Name = "Expenses Budget Details", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1D2", Name = "Expenses Budget Details", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1D3", Name = "Expenses Budget Details", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1D4", Name = "Expenses Budget Details", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1D5", Name = "Expenses Budget Details", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1D6", Name = "Expenses Budget Details", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1D7", Name = "Expenses Budget Details", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Expenses Budget Details

      #region Issues Attachments

      new DefaultAccessRight { Id = "ENP-1E1", Name = "Issues Attachments", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1E2", Name = "Issues Attachments", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1E3", Name = "Issues Attachments", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1E4", Name = "Issues Attachments", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1E5", Name = "Issues Attachments", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1E6", Name = "Issues Attachments", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1E7", Name = "Issues Attachments", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Issues Attachments

      #region Issues Progresses

      new DefaultAccessRight { Id = "ENP-1F1", Name = "Issues Progresses", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1F2", Name = "Issues Progresses", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1F3", Name = "Issues Progresses", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1F4", Name = "Issues Progresses", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1F5", Name = "Issues Progresses", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1F6", Name = "Issues Progresses", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1F7", Name = "Issues Progresses", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Issues Progresses

      #region Issues Submissions

      new DefaultAccessRight { Id = "ENP-1G1", Name = "Issues Submissions", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1G2", Name = "Issues Submissions", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1G3", Name = "Issues Submissions", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1G4", Name = "Issues Submissions", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1G5", Name = "Issues Submissions", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1G6", Name = "Issues Submissions", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1G7", Name = "Issues Submissions", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Issues Submissions

      #region Item Groups

      new DefaultAccessRight { Id = "ENP-1H1", Name = "Item Groups", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1H2", Name = "Item Groups", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1H3", Name = "Item Groups", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1H4", Name = "Item Groups", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1H5", Name = "Item Groups", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1H6", Name = "Item Groups", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1H7", Name = "Item Groups", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Item Groups

      #region Leased Properties Accounts

      new DefaultAccessRight { Id = "ENP-1I1", Name = "Leased Properties Accounts", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1I2", Name = "Leased Properties Accounts", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1I3", Name = "Leased Properties Accounts", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1I4", Name = "Leased Properties Accounts", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1I5", Name = "Leased Properties Accounts", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1I6", Name = "Leased Properties Accounts", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1I7", Name = "Leased Properties Accounts", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Leased Properties Accounts

      #region Ledger Accounts

      new DefaultAccessRight { Id = "ENP-1J1", Name = "Ledger Accounts", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1J2", Name = "Ledger Accounts", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1J3", Name = "Ledger Accounts", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1J4", Name = "Ledger Accounts", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1J5", Name = "Ledger Accounts", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1J6", Name = "Ledger Accounts", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1J7", Name = "Ledger Accounts", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Ledger Accounts

      #region Let Properties Accounts

      new DefaultAccessRight { Id = "ENP-1K1", Name = "Let Properties Accounts", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1K2", Name = "Let Properties Accounts", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1K3", Name = "Let Properties Accounts", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1K4", Name = "Let Properties Accounts", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1K5", Name = "Let Properties Accounts", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1K6", Name = "Let Properties Accounts", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1K7", Name = "Let Properties Accounts", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Let Properties Accounts

      #region Password Safes

      new DefaultAccessRight { Id = "ENP-1L1", Name = "Password Safes", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1L2", Name = "Password Safes", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1L3", Name = "Password Safes", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1L4", Name = "Password Safes", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1L5", Name = "Password Safes", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1L6", Name = "Password Safes", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1L7", Name = "Password Safes", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Password Safes

      #region Payroll Groups

      new DefaultAccessRight { Id = "ENP-1M1", Name = "Payroll Groups", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1M2", Name = "Payroll Groups", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1M3", Name = "Payroll Groups", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1M4", Name = "Payroll Groups", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1M5", Name = "Payroll Groups", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1M6", Name = "Payroll Groups", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1M7", Name = "Payroll Groups", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Payroll Groups

      #region Price Change Requests

      new DefaultAccessRight { Id = "ENP-1N1", Name = "Price Change Requests", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1N2", Name = "Price Change Requests", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1N3", Name = "Price Change Requests", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1N4", Name = "Price Change Requests", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1N5", Name = "Price Change Requests", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1N6", Name = "Price Change Requests", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1N7", Name = "Price Change Requests", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Price Change Requests

      #region Project Issues

      new DefaultAccessRight { Id = "ENP-1O1", Name = "Project Issues", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1O2", Name = "Project Issues", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1O3", Name = "Project Issues", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1O4", Name = "Project Issues", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1O5", Name = "Project Issues", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1O6", Name = "Project Issues", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1O7", Name = "Project Issues", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Project Issues

      #region Purchases Budget Batch Headers

      new DefaultAccessRight { Id = "ENP-1P1", Name = "Purchases Budget Batch Headers", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1P2", Name = "Purchases Budget Batch Headers", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1P3", Name = "Purchases Budget Batch Headers", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1P4", Name = "Purchases Budget Batch Headers", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1P5", Name = "Purchases Budget Batch Headers", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1P6", Name = "Purchases Budget Batch Headers", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1P7", Name = "Purchases Budget Batch Headers", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Purchases Budget Batch Headers

      #region Purchases Budget Details

      new DefaultAccessRight { Id = "ENP-1Q1", Name = "Purchases Budget Details", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Q2", Name = "Purchases Budget Details", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Q3", Name = "Purchases Budget Details", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Q4", Name = "Purchases Budget Details", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Q5", Name = "Purchases Budget Details", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Q6", Name = "Purchases Budget Details", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Q7", Name = "Purchases Budget Details", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Purchases Budget Details

      #region QR Codes Requests

      new DefaultAccessRight { Id = "ENP-1R1", Name = "QR Codes Requests", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1R2", Name = "QR Codes Requests", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1R3", Name = "QR Codes Requests", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1R4", Name = "QR Codes Requests", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1R5", Name = "QR Codes Requests", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1R6", Name = "QR Codes Requests", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1R7", Name = "QR Codes Requests", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion QR Codes Requests

      #region QR Request Items

      new DefaultAccessRight { Id = "ENP-1S1", Name = "QR Request Items", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1S2", Name = "QR Request Items", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1S3", Name = "QR Request Items", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1S4", Name = "QR Request Items", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1S5", Name = "QR Request Items", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1S6", Name = "QR Request Items", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1S7", Name = "QR Request Items", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion QR Request Items

      #region Sales Budget Batch Headers

      new DefaultAccessRight { Id = "ENP-1T1", Name = "Sales Budget Batch Headers", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1T2", Name = "Sales Budget Batch Headers", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1T3", Name = "Sales Budget Batch Headers", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1T4", Name = "Sales Budget Batch Headers", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1T5", Name = "Sales Budget Batch Headers", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1T6", Name = "Sales Budget Batch Headers", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1T7", Name = "Sales Budget Batch Headers", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Sales Budget Batch Headers

      #region Sales Budget Details

      new DefaultAccessRight { Id = "ENP-1U1", Name = "Sales Budget Details", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1U2", Name = "Sales Budget Details", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1U3", Name = "Sales Budget Details", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1U4", Name = "Sales Budget Details", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1U5", Name = "Sales Budget Details", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1U6", Name = "Sales Budget Details", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1U7", Name = "Sales Budget Details", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Sales Budget Details

      #region Staff Groups

      new DefaultAccessRight { Id = "ENP-1V1", Name = "Staff Groups", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1V2", Name = "Staff Groups", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1V3", Name = "Staff Groups", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1V4", Name = "Staff Groups", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1V5", Name = "Staff Groups", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1V6", Name = "Staff Groups", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1V7", Name = "Staff Groups", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Staff Groups

      #region Stock Count Sheets

      new DefaultAccessRight { Id = "ENP-1W1", Name = "Stock Count Sheets", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1W2", Name = "Stock Count Sheets", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1W3", Name = "Stock Count Sheets", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1W4", Name = "Stock Count Sheets", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1W5", Name = "Stock Count Sheets", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1W6", Name = "Stock Count Sheets", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1W7", Name = "Stock Count Sheets", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Stock Count Sheets

      #region Stock Item Codes Requests

      new DefaultAccessRight { Id = "ENP-1X1", Name = "Stock Item Codes Requests", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1X2", Name = "Stock Item Codes Requests", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1X3", Name = "Stock Item Codes Requests", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1X4", Name = "Stock Item Codes Requests", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1X5", Name = "Stock Item Codes Requests", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1X6", Name = "Stock Item Codes Requests", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1X7", Name = "Stock Item Codes Requests", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Stock Item Codes Requests

      #region Stock Items

      new DefaultAccessRight { Id = "ENP-1Y1", Name = "Stock Items", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Y2", Name = "Stock Items", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Y3", Name = "Stock Items", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Y4", Name = "Stock Items", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Y5", Name = "Stock Items", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Y6", Name = "Stock Items", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Y7", Name = "Stock Items", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Stock Items

      #region Suppliers

      new DefaultAccessRight { Id = "ENP-1Z1", Name = "Suppliers", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Z2", Name = "Suppliers", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Z3", Name = "Suppliers", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Z4", Name = "Suppliers", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Z5", Name = "Suppliers", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-1Z6", Name = "Suppliers", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-1Z7", Name = "Suppliers", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Suppliers

      #region System Rights

      new DefaultAccessRight { Id = "ENP-201", Name = "System Rights", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-202", Name = "System Rights", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-203", Name = "System Rights", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-204", Name = "System Rights", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-205", Name = "System Rights", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-206", Name = "System Rights", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-207", Name = "System Rights", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion System Rights

      #region System Users

      new DefaultAccessRight { Id = "ENP-211", Name = "System Users", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-212", Name = "System Users", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-213", Name = "System Users", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-214", Name = "System Users", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-215", Name = "System Users", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-216", Name = "System Users", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-217", Name = "System Users", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion System Users

      #region Tims Machines

      new DefaultAccessRight { Id = "ENP-221", Name = "Tims Machines", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-222", Name = "Tims Machines", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-223", Name = "Tims Machines", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-224", Name = "Tims Machines", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-225", Name = "Tims Machines", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-226", Name = "Tims Machines", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-227", Name = "Tims Machines", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Tims Machines

      #region User Audit Trails

      new DefaultAccessRight { Id = "ENP-231", Name = "User Audit Trails", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-232", Name = "User Audit Trails", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-233", Name = "User Audit Trails", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-234", Name = "User Audit Trails", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-235", Name = "User Audit Trails", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-236", Name = "User Audit Trails", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-237", Name = "User Audit Trails", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion User Audit Trails

      #region User Logins

      new DefaultAccessRight { Id = "ENP-241", Name = "User Logins", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-242", Name = "User Logins", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-243", Name = "User Logins", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-244", Name = "User Logins", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-245", Name = "User Logins", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-246", Name = "User Logins", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-247", Name = "User Logins", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion User Logins

      #region User Rights

      new DefaultAccessRight { Id = "ENP-251", Name = "User Rights", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-252", Name = "User Rights", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-253", Name = "User Rights", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-254", Name = "User Rights", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-255", Name = "User Rights", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-256", Name = "User Rights", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-257", Name = "User Rights", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion User Rights

      #region User Roles

      new DefaultAccessRight { Id = "ENP-261", Name = "User Roles", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-262", Name = "User Roles", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-263", Name = "User Roles", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-264", Name = "User Roles", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-265", Name = "User Roles", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-266", Name = "User Roles", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-267", Name = "User Roles", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion User Roles

      #region Vendor Codes Requests

      new DefaultAccessRight { Id = "ENP-271", Name = "Vendor Codes Requests", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-272", Name = "Vendor Codes Requests", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-273", Name = "Vendor Codes Requests", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-274", Name = "Vendor Codes Requests", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-275", Name = "Vendor Codes Requests", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-276", Name = "Vendor Codes Requests", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-277", Name = "Vendor Codes Requests", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Vendor Codes Requests

      #region Verification Rights

      new DefaultAccessRight { Id = "ENP-281", Name = "Verification Rights", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-282", Name = "Verification Rights", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-283", Name = "Verification Rights", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-284", Name = "Verification Rights", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-285", Name = "Verification Rights", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-286", Name = "Verification Rights", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-287", Name = "Verification Rights", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Verification Rights

      #region Verification Types

      new DefaultAccessRight { Id = "ENP-291", Name = "Verification Types", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-292", Name = "Verification Types", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-293", Name = "Verification Types", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-294", Name = "Verification Types", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-295", Name = "Verification Types", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-296", Name = "Verification Types", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-297", Name = "Verification Types", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Verification Types

      #region Verifications

      new DefaultAccessRight { Id = "ENP-2A1", Name = "Verifications", Type = "Create", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-2A2", Name = "Verifications", Type = "Delete", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-2A3", Name = "Verifications", Type = "Dynamic Get", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-2A4", Name = "Verifications", Type = "Get By Id", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-2A5", Name = "Verifications", Type = "Get Many", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES},{UserRoleConstants.ROLE_MANAGER}" },
      new DefaultAccessRight { Id = "ENP-2A6", Name = "Verifications", Type = "Patch", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },
      new DefaultAccessRight { Id = "ENP-2A7", Name = "Verifications", Type = "Update", Rights = $"{UserRoleConstants.RIGHT_SYSTEM_ROUTINES}" },

      #endregion Verifications

    ];
    var existingIds = db.DefaultAccessRights
      .Where(c => groups.Select(c => c.Id)
      .ToArray().Contains(c.Id))
      .Select(m => m.Id).ToList();

    groups = groups.Where(c => !existingIds.Contains(c.Id)).ToArray();
    if (groups.Length != 0)
    {
      await db.DefaultAccessRights.AddRangeAsync(groups);
      await db.SaveChangesAsync();
    }
    return true;
  }
}
