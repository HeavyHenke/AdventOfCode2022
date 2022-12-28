using MoreLinq.Extensions;

class Day23
{
    public object A()
    {
        var elves = Enumerable.ToHashSet(File.ReadLines("Day23.txt")
            .Index()
            .SelectMany(l => l.Value.Index().Select(c => (y: l.Key, x: c.Key, val: c.Value)))
            .Where(c => c.val == '#')
            .Select(c => (c.x, c.y)));

        var checkAndMoveDirs = new[]
        {
            new[] { (dx: 0, dy: -1), (dx: -1, dy: -1), (dx: 1, dy: -1) },
            new[] { (dx: 0, dy: 1), (dx: -1, dy: 1), (dx: 1, dy: 1) },
            new[] { (dx: -1, dy: 0), (dx: -1, dy: -1), (dx: -1, dy: 1) },
            new[] { (dx: 1, dy: 0), (dx: 1, dy: -1), (dx: 1, dy: 1) },
        };
        var allDirs = checkAndMoveDirs.SelectMany(d => d).Distinct().ToArray();
        
        int dirOrder = 0;

        (int x, int y)? ProposeNewPos((int x, int y) oldPos)
        {
            if (!allDirs.Any(d => elves.Contains((oldPos.x + d.dx, oldPos.y + d.dy))))
                return null;    // No need to move
            
            var dir = dirOrder;
            for (int p = 0; p < 4; p++)
            {
                bool goodPos = checkAndMoveDirs[dir].Select(pp => (oldPos.x + pp.dx, oldPos.y + pp.dy)).All(pp => elves.Contains(pp) == false);
                if (goodPos)
                    return (oldPos.x + checkAndMoveDirs[dir][0].dx, oldPos.y + checkAndMoveDirs[dir][0].dy);
                dir++;
                dir &= 0x03;
            }

            return null;
        }
        
        Print(elves);

        for (int i = 0; i < 10; i++)
        {
            var proposedPositions = elves.Select(e => (e, ProposeNewPos(e))).ToList();
            var doublePoses = Enumerable.ToHashSet(proposedPositions.Where(p => p.Item2.HasValue).Select(p => p.Item2).ToLookup(key => key.Value).Where(l => l.Count() > 1).Select(g => g.Key));

            elves = Enumerable.ToHashSet(proposedPositions.Select(p => p.Item2 == null || doublePoses.Contains(p.Item2.Value) ? p.e : p.Item2.Value));
            dirOrder++;
            dirOrder &= 0x03;
            
            // Print(elves);
        }

        int minX = elves.Min(e => e.x);
        int maxX = elves.Max(e => e.x);
        int minY = elves.Min(e => e.y);
        int maxY = elves.Max(e => e.y);

        return (maxX - minX + 1) * (maxY - minY + 1) - elves.Count;
    }

    public object B()
    {
        var elves = Enumerable.ToHashSet(File.ReadLines("Day23.txt")
            .Index()
            .SelectMany(l => l.Value.Index().Select(c => (y: l.Key, x: c.Key, val: c.Value)))
            .Where(c => c.val == '#')
            .Select(c => (c.x, c.y)));

        var checkAndMoveDirs = new[]
        {
            new[] { (dx: 0, dy: -1), (dx: -1, dy: -1), (dx: 1, dy: -1) },
            new[] { (dx: 0, dy: 1), (dx: -1, dy: 1), (dx: 1, dy: 1) },
            new[] { (dx: -1, dy: 0), (dx: -1, dy: -1), (dx: -1, dy: 1) },
            new[] { (dx: 1, dy: 0), (dx: 1, dy: -1), (dx: 1, dy: 1) },
        };
        var allDirs = checkAndMoveDirs.SelectMany(d => d).Distinct().ToArray();

        int dirOrder = 0;

        (int x, int y)? ProposeNewPos((int x, int y) oldPos)
        {
            if (!allDirs.Any(d => elves.Contains((oldPos.x + d.dx, oldPos.y + d.dy))))
                return null; // No need to move

            var dir = dirOrder;
            for (int p = 0; p < 4; p++)
            {
                bool goodPos = checkAndMoveDirs[dir].Select(pp => (oldPos.x + pp.dx, oldPos.y + pp.dy)).All(pp => elves.Contains(pp) == false);
                if (goodPos)
                    return (oldPos.x + checkAndMoveDirs[dir][0].dx, oldPos.y + checkAndMoveDirs[dir][0].dy);
                dir++;
                dir &= 0x03;
            }

            return null;
        }

        int round = 1;
        while (true)
        {
            var proposedPositions = elves.Select(e => (e, ProposeNewPos(e))).ToList();
            if (proposedPositions.All(p => p.Item2 == null))
                return round;

            var doublePoses = Enumerable.ToHashSet(proposedPositions.Where(p => p.Item2.HasValue).Select(p => p.Item2).ToLookup(key => key.Value).Where(l => l.Count() > 1).Select(g => g.Key));

            elves = Enumerable.ToHashSet(proposedPositions.Select(p => p.Item2 == null || doublePoses.Contains(p.Item2.Value) ? p.e : p.Item2.Value));
            dirOrder++;
            dirOrder &= 0x03;

            round++;
        }
    }

    private static void Print(HashSet<(int x, int y)> elves)
    {
        int minX = elves.Min(e => e.x);
        int maxX = elves.Max(e => e.x);
        int minY = elves.Min(e => e.y);
        int maxY = elves.Max(e => e.y);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if(elves.Contains((x,y)))
                    Console.Write('#');
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}