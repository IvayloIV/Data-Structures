using BunnyWars.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SuffixOrder : IComparer<Bunny>
{
    public int Compare(Bunny x, Bunny y)
    {
        var xLength = x.Name.Length - 1;
        var yLength = y.Name.Length - 1;

        while (xLength >= 0 && yLength >= 0)
        {
            var cmp = x.Name[xLength].CompareTo(y.Name[yLength]);
            if (cmp != 0)
            {
                return cmp;
            }

            xLength--;
            yLength--;
        }

        return x.Name.Length.CompareTo(y.Name.Length);
    }
}