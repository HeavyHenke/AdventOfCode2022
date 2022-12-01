using MoreLinq;

namespace AdventOfCode2022;

public class Day1
{
    public object A()
    {
        var lines = File.ReadLines("Day1.txt");
        
        int maxCals = 0;
        int cals = 0;

        foreach (var l in lines)
        {
            if (string.IsNullOrEmpty(l))
            {
                if (cals > maxCals)
                    maxCals = cals;
                cals = 0;
                continue;
            }

            cals += int.Parse(l);
        }
        if (cals > maxCals)
            maxCals = cals;

        return maxCals;
    }
    
    public object B()
    {
        return File
            .ReadLines("Day1.txt")
            .Select(l => l == "" ? null : l)
            .Segment(string.IsNullOrEmpty)
            .Select(seg => seg.Select(c => int.Parse(c ?? "0")).Sum())
            .OrderByDescending(o => o)
            .Take(3)
            .Sum();
    }
}