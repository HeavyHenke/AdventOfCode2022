class Day4
{
    public object A()
    {
        return File.ReadLines("Day4.txt")
            .Select(l => l.Split(new[] { '-', ',' }))
            .Select(l => l.Select(int.Parse).ToArray())
            .Count(l => Overlaps(l[0], l[1], l[2], l[3]) || Overlaps(l[2], l[3], l[0], l[1]));
    }

    private static  bool Overlaps(int a1, int a2, int b1, int b2)
    {
        return a1 >= b1 && a1 <= b2 && a2 <= b2 && a2 >= b1;
    }

    public object B()
    {
        return File.ReadLines("Day4.txt")
            .Select(l => l.Split(new[] { '-', ',' }))
            .Select(l => l.Select(int.Parse).ToArray())
            .Count(l => Enumerable.Range(l[0], l[1] - l[0] + 1).Intersect(Enumerable.Range(l[2], l[3] - l[2] + 1)).Any());
    }
}