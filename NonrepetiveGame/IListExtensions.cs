using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonrepetiveGame
{
    public static class IListExtensions
    {
        private static readonly Random r = new();
        
        public static T GetRandomElement<T>(this IList<T> list)
        {
            return list[r.Next(list.Count)];
        }
    }
}
