using System.Collections.Generic;
using System.Linq;

namespace uRADMonitorX.Extensions
{
    public static class ListExtensions
    {
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count() == 0;
        }
    }
}
