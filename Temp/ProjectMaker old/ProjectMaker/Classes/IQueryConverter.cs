using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectMaker.Classes
{
    public interface IQueryConverter
    {
        object Convert<T>(object query);
    }
}
