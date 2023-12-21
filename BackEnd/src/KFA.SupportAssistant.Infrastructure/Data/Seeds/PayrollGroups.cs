using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFA.SupportAssistant.Core.Models;

namespace KFA.SupportAssistant.Infrastructure.Data.Seeds;
internal static class PayrollGroups
{
   public static async Task<bool> Process(AppDbContext db)
  {
    PayrollGroup[] groups = [
      new PayrollGroup { GroupName = "Unionisable", Id = "U" },
      new PayrollGroup { GroupName = "Middle Management", Id = "M" },
      new PayrollGroup { GroupName = "Senior Management", Id = "S" }
      ];
    var existingIds = db.PayrollGroups
      .Where(c => groups.Select(c => c.Id)
      .ToArray().Contains(c.Id))
      .Select(m => m.Id).ToList();

    groups = groups.Where(c => !existingIds.Contains(c.Id)).ToArray();
    if (groups.Length != 0)
    {
      await db.PayrollGroups.AddRangeAsync(groups);
      await db.SaveChangesAsync();
    }

    StaffGroup[] sGroups = [
      new StaffGroup { Description = "On Hold", Id = "Hold", IsActive = false },
      new StaffGroup { Description = "Suspended", Id = "Suspended", IsActive = false },
      new StaffGroup { Description = "Retrenched", Id = "Retrenched", IsActive = false },
      new StaffGroup { Description = "Retired", Id = "Retired", IsActive = false },
      new StaffGroup { Description = "Active", Id = "Active", IsActive = true },
      new StaffGroup { Description = "Terminated", Id = "Terminated", IsActive = false }
      ];
    existingIds =
    [
      .. db.StaffGroups
            .Where(c => sGroups.Select(c => c.Id)
            .ToArray().Contains(c.Id))
            .Select(m => m.Id),
    ];

    sGroups = sGroups.Where(c => !existingIds.Contains(c.Id)).ToArray();
    if (sGroups.Length != 0)
    {
      await db.StaffGroups.AddRangeAsync(sGroups);
      await db.SaveChangesAsync();
    }
    return true;
  }
}
