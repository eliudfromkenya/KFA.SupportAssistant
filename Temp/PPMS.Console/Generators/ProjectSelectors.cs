using System;
using System.Collections.Generic;
using System.Linq;
using PPMS.Console.Models;

namespace PPMS.Console.Generators;

static class ProjectSelectors
{
  public static void Generate()
  {
    using var db = new Data.Context();
    db.Database.EnsureCreated();

    var lev1 = from master in db.Columns
               from foreign in db.Columns
               from tbl in db.Tables
               from rel in db.Relations
               where rel.MasterColumnId == master.Id &&
               tbl.Name == "Projects" &&
               rel.ForeignColumnId == foreign.Id &&
               master.TableId == tbl.Id
               select new { fCol = foreign.ColumnName, mCol = master.ColumnName, foreign.Table.Name };

    var m1 = lev1.Distinct().ToArray();


    var lev2 = from master in db.Columns
               from foreign in db.Columns
               from tbl in db.Tables
               from rel in db.Relations
               where rel.MasterColumnId == master.Id &&
               m1.Select(x => x.Name).ToArray().Any(x => x == tbl.Name) &&
              rel.ForeignColumnId == foreign.Id &&
              master.TableId == tbl.Id
               select new { fCol = foreign.ColumnName, mTbl = master.Table.Name, mCol = master.ColumnName, foreign.Table.Name };

    var m2 = lev2.Distinct().ToArray();
    //var lev3 = from master in db.Columns
    //           from foreign in db.Columns
    //           from tbl in db.Tables
    //           from rel in db.Relations
    //           where rel.MasterColumnId == master.Id &&
    //          rel.ForeignColumnId == foreign.Id &&
    //          master.TableId == tbl.Id
    //           select new
    //           {
    //               fCol = foreign.ColumnName,
    //               mTbl = master.Table.Name,
    //               mCol = master.ColumnName,
    //               foreign.Table.Name,
    //           };

    //var lev4 = from oldName in m2.Select(x => x.Name).ToArray()
    //           from lev in lev3.ToArray()
    //           where lev.mTbl == oldName
    //           && lev.Name != oldName
    //           select new
    //           {
    //               lev.fCol, lev.mCol,
    //               lev.mTbl, lev.Name, oldName
    //           };

    //var m3 = lev4.Distinct().ToArray();

    var dic = new List<Tuple<string, string, int>>();

    foreach (var obj in m1)
    {
      var singular = Functions.Singularize(obj.Name).MakeName();
      dic.Add(new Tuple<string, string, int>(singular, string.Format(@" projectIds.Contains(x.{1})", singular, obj.fCol.MakeName()), 0));
    }

    foreach (var obj in m2)
    {
      var singular = Functions.Singularize(obj.Name).MakeName();
      var name = (obj.fCol.ToLower().EndsWith(" id") ? obj.fCol.Substring(0, obj.fCol.Length - 3).Trim() : obj.fCol).MakeName();

      if (dic.Any(x => x.Item1 == singular))
      {
        var objX = dic.First(x => x.Item1 == singular);
        dic.Remove(objX);

        var bodyX = objX.Item2 + " || " + string.Format(@"projectIds.Contains(x.{1}.ProjectID)", singular, name);
        dic.Add(new Tuple<string, string, int>(singular, bodyX, 0));
      }
      else
        dic.Add(new Tuple<string, string, int>(singular, string.Format(@" projectIds.Contains(x.{1}.ProjectID)", singular, name), 0));
    }

    //foreach (var obj in m3)
    //{

    //}

    var text = string.Join("\r\n\r\n", dic.Select(x => string.Format(@"
            if (query.ElementType == typeof({0}))
            {{

                if (query is IQueryable<{0}> qry)
                {{
                    qry = qry.Where(x => {1});
                    return qry as IQueryable<T>;
                }}
            }}", x.Item1, x.Item2)));
  }
}
