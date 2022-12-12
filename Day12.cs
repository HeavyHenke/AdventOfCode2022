class Day12
{
    public object A()
    {
        var map = ReadMapAndPositions(out var startPos, out var goalPos, "Day12.txt");

        return FindClosestPath(map, startPos, goalPos);
    }

    public object B()
    {
        var map = ReadMapAndPositions(out _, out var goalPos, "Day12.txt");

        int bestRouteLength = int.MaxValue;
        
        for (int y = 0; y < map.Length; y++)
        for (int x = 0; x < map[y].Length; x++)
        {
            if (map[y][x] == 'a')
            {
                var length = FindClosestPath(map, (x, y), goalPos);
                bestRouteLength = Math.Min(bestRouteLength, length);
            }
        }

        return bestRouteLength;
    }


    private static char[][] ReadMapAndPositions(out (int x, int y) startPos, out (int x, int y) goalPos, string fileName)
    {
        var map = File.ReadAllLines(fileName).Select(l => l.ToArray()).ToArray();

        startPos = (-1, -1);
        goalPos = (-1, -1);

        for (int y = 0; y < map.Length; y++)
        for (int x = 0; x < map[y].Length; x++)
        {
            if (map[y][x] == 'S')
            {
                startPos = (x, y);
                map[y][x] = 'a';
            }
            else if (map[y][x] == 'E')
            {
                goalPos = (x, y);
                map[y][x] = 'z';
            }
        }

        return map;
    }

    private static int FindClosestPath(char[][] map, (int x, int y) startPos, (int x, int y) goalPos)
    {
        var visited = new HashSet<(int x, int y)> { startPos };

        var searchQueue = new Queue<((int x, int y) poa, int steps)>();
        searchQueue.Enqueue((startPos, 1));

        while (searchQueue.Any())
        {
            var (pos, steps) = searchQueue.Dequeue();

            foreach (var n in GetNeighbours(pos, map))
            {
                if (map[n.y][n.x] > map[pos.y][pos.x] + 1)
                    continue; // To steep

                if (visited.Add(n) == false)
                    continue;

                if (n == goalPos)
                {
                    return steps;
                }

                searchQueue.Enqueue((n, steps + 1));
            }
        }

        return int.MaxValue;
    }

    private static IEnumerable<(int x, int y)> GetNeighbours((int x, int y) pos, char[][] map)
    {
        if (pos.x > 0)
            yield return (pos.x - 1, pos.y);
        if (pos.y > 0)
            yield return (pos.x, pos.y - 1);
        if (pos.x < map[0].Length - 1)
            yield return (pos.x + 1, pos.y);
        if (pos.y < map.Length - 1)
            yield return (pos.x, pos.y + 1);
    }
}