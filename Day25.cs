using MoreLinq.Extensions;

class Day25
{
    public object A()
    {
        var dict = new Dictionary<char, int>
        {
            { '2', 2 },
            { '1', 1 },
            { '0', 0 },
            { '-', -1 },
            { '=', -2 }
        };
        var revDict = dict.ToDictionary(key => key.Value, val => val.Key);
        var lines = File.ReadAllLines("Day25.txt");
        var maxLength = lines.Max(l => l.Length);

        var data = lines
            .Select(l => l.Select(c => dict[c]).PadStart(maxLength))    // Translate to numbers and pad it making all rows equally long
            .Transpose()
            .Select(l => l.Sum())   // Sum all columns (since it is transposed now)
            .Reverse()
            .ToList();

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i] > 0)
            {
                var overflow = (data[i] + 2) / 5;
                data[i] -= overflow * 5;
                AddValue(data, i + 1, overflow);
            }
            else if (data[i] < 0)
            {
                var overflow = (data[i] - 2) / 5;
                data[i] -= overflow * 5;
                AddValue(data, i + 1, overflow);
            }
        }

        var result = new string(data.Select(d => revDict[d]).Reverse().ToArray()).TrimStart('0');
        return result;
    }

    private static void AddValue(List<int> base5, int ix, int val)
    {
        if (ix > base5.Count - 1)
            base5.Add(val);
        else
            base5[ix] += val;
    }
}