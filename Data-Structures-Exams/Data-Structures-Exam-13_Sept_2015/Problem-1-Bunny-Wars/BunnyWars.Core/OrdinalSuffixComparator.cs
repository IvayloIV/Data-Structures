using BunnyWars.Core;
using System.Collections.Generic;

public class OrdinalSuffixComparator : IComparer<Bunny>
{
    public int Compare(Bunny bunnyX, Bunny bunnyY)
    {
        var xBunnyName = bunnyX.Name;
        var yBunnyName = bunnyY.Name;

        var xCount = xBunnyName.Length - 1;
        var yCount = yBunnyName.Length - 1;
        while (xCount >= 0 && yCount >= 0)
        {
            if (xBunnyName[xCount] > yBunnyName[yCount])
            {
                return 1;
            }
            else if (xBunnyName[xCount] < yBunnyName[yCount])
            {
                return -1;
            }
            xCount--;
            yCount--;
        }

        return xBunnyName.Length.CompareTo(yBunnyName.Length);
    }
}