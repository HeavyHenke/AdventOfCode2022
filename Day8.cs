using MoreLinq.Extensions;

class Day8
{
    public object A()
    {
        var grid = File.ReadLines("Day8.txt")
            .Index()
            .SelectMany(row => row.Value.Index().Select(kvp => (row.Key, kvp.Key, kvp.Value - '0')))
            .ToDictionary(key => (x: key.Item1, y: key.Item2), val => val.Item3);
        int maxy = grid.Keys.Max(k => k.y);
        int maxx = grid.Keys.Max(k => k.x);

        var visible = new HashSet<(int x, int y)>();

        for (int y = 0; y <= maxy; y++)
        {
            int highest = -1;
            for (int x = 0; x <= maxx; x++)
            {
                if (grid[(x, y)] > highest)
                {
                    highest = grid[(x, y)];
                    visible.Add((x, y));
                }
            }

            highest = -1;
            for (int x = maxx; x >= 0; x--)
            {
                if (grid[(x, y)] > highest)
                {
                    highest = grid[(x, y)];
                    visible.Add((x, y));
                }
            }
        }

        for (int x = 0; x <= maxx; x++)
        {
            int highest = -1;
            for (int y = 0; y <= maxy; y++)
            {
                if (grid[(x, y)] > highest)
                {
                    highest = grid[(x, y)];
                    visible.Add((x, y));
                }
            }

            highest = -1;
            for (int y = maxy; y >= 0; y--)
            {
                if (grid[(x, y)] > highest)
                {
                    highest = grid[(x, y)];
                    visible.Add((x, y));
                }
            }
        }

        return visible.Count;
    }

    public object B()
    {
        var grid = File.ReadLines("Day8.txt")
            .Index()
            .SelectMany(row => row.Value.Index().Select(kvp => (kvp.Key, row.Key, kvp.Value - '0')))
            .ToDictionary(key => (x: key.Item1, y: key.Item2), val => val.Item3);
        int maxY = grid.Keys.Max(k => k.y);
        int maxX = grid.Keys.Max(k => k.x);

        return Enumerable.Range(1, maxX - 1)
            .Cartesian(Enumerable.Range(1, maxY - 1), (x, y) => (x, y)) // All x,y values
            .Select(coord => // For each coord
                    Move(coord.x, coord.y, maxX, maxY) // In all directions
                        .Select(moveDir => moveDir.Select(c => grid[c])
                            .TakeUntil(c => c >= grid[coord])
                            .Count()) // Distance per direction
                        .Aggregate(1, (current, t) => current * t) // Calc score by multiplying
            ).Max();
    }

    private static IEnumerable<IEnumerable<(int x, int y)>> Move(int startX, int startY, int maxX, int maxY)
    {
        yield return MoveX(startX, startY, maxX + 1, 1);
        yield return MoveX(startX, startY, -1, -1);
        yield return MoveY(startX, startY, maxY + 1, 1);
        yield return MoveY(startX, startY, -1, -1);
    }

    private static IEnumerable<(int x, int y)> MoveX(int startX, int startY, int stopX, int dx)
    {
        for (int x = startX + dx; x != stopX; x += dx)
            yield return (x, startY);
    }

    private static IEnumerable<(int x, int y)> MoveY(int startX, int startY, int stopY, int dy)
    {
        for (int y = startY + dy; y != stopY; y += dy)
            yield return (startX, y);
    }
}