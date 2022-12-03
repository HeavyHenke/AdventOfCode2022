using MoreLinq;

class Day3
{
    public object A()
    {
        return File.ReadLines("day3.txt")
            .Select(l => l.Take(l.Length / 2).Intersect(l.Skip(l.Length / 2)))
            .Select(l => l.Single())
            .Select(l => (l >= 'a') ? l - 'a' + 1 : l - 'A' + 27)
            .Sum();
    }

    public object B()
    {
        return File.ReadLines("day3.txt")
            .Batch(3)
            .Select(b => b.ToList())
            .Select(b => b[0].Intersect(b[1]).Intersect(b[2]).Single())
            .Select(l => (l >= 'a') ? l - 'a' + 1 : l - 'A' + 27)
            .Sum();
    }
}