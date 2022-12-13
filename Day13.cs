using MoreLinq;

class Day13
{
    public object A()
    {
        return File.ReadLines("Day13.txt")
            .Where(l => !string.IsNullOrEmpty(l))
            .Select(ParseList)
            .Batch(2)
            .Select(b => b.ToArray())
            .Index()
            .Where(b => Compare(b.Value[0], b.Value[1]) == -1)
            .Select(b => b.Key + 1)
            .Sum();
    }

    public object B()
    {
        var dividerPackages = new[] { ParseList("[[2]]"), ParseList("[[6]]") };
        var allPackets = File.ReadLines("Day13.txt")
            .Where(l => !string.IsNullOrEmpty(l))
            .Select(ParseList)
            .Concat(dividerPackages)
            .ToList();

        allPackets.Sort(Compare);
        return (allPackets.IndexOf(dividerPackages[0]) + 1) * (allPackets.IndexOf(dividerPackages[1]) + 1);
    }

    private static List<object> ParseList(string str)
    {
        var zero = 0;
        return ParseList(str, ref zero);
    }
    
    private static List<object> ParseList(string str, ref int ix)
    {
        var ret = new List<object>();
        while (str[++ix] != ']')
        {
            if (str[ix] == ',')
                ix++;
            if (str[ix] == '[')
            {
                ret.Add(ParseList(str, ref ix));
            }
            else
            {
                var numChars = str[ix..].TakeWhile(char.IsDigit).Count();
                var num = int.Parse(str[ix..(ix + numChars)]);
                ret.Add(num);
                ix += numChars - 1;
            }
        }

        return ret;
    }

    private static int Compare(IEnumerable<object> a, IEnumerable<object> b)
    {
        using var enumeratorA = a.GetEnumerator();
        using var enumeratorB = b.GetEnumerator();

        while (true)
        {
            var aHasNext = enumeratorA.MoveNext();
            var bHasNext = enumeratorB.MoveNext();
            if (!aHasNext && !bHasNext)
                return 0;
            if (aHasNext && !bHasNext)
                return 1;
            if (!aHasNext)
                return -1;

            if (enumeratorA.Current is int numA && enumeratorB.Current is int numB)
            {
                if(numA == numB)
                    continue;
                return numA.CompareTo(numB);
            }

            var l1 = enumeratorA.Current as List<object> ?? new List<object> { (int)enumeratorA.Current };
            var l2 = enumeratorB.Current as List<object> ?? new List<object> { (int)enumeratorB.Current };
            var childCompareResult = Compare(l1, l2);
            if (childCompareResult != 0)
                return childCompareResult;
        }
    }
}