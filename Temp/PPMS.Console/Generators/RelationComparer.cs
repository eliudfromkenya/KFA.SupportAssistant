using System;
using System.Collections.Generic;
using PPMS.Console.Models;

namespace PPMS.Console.Generators {
    public class RelationComparer : EqualityComparer<TableRelation> {
        public override bool Equals (TableRelation x, TableRelation y) {
            return x.MasterColumn.Id == y.MasterColumn.Id &&
                x.ForeignColumn.Id == y.ForeignColumn.Id;
        }

        public override int GetHashCode (TableRelation obj) {
            return obj.GetHashCode ();
        }
    }
}