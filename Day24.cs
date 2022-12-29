using MoreLinq;

class Day24
{
    public object A()
    {
        var map = File.ReadAllLines("Day24.txt");
        var blizzards = map
            .Index()
            .SelectMany(l => l.Value.Index().Select(q => (x: q.Key, y: l.Key, dir: q.Value)))
            .Where(b => b.dir is '<' or '>' or 'v' or '^')
            .ToLookup(key => (key.x, key.y), val => val.dir);
        
        var startY = 0;
        var startX = map[startY].IndexOf('.');
        var goalY = map.Length - 1;
        var goalX = map[goalY].IndexOf('.');

        return CalcFastestRoute(startX, startY, goalX, goalY, ref blizzards, map);
    }

    public object B()
    {
        var map = File.ReadAllLines("Day24.txt");
        var blizzards = map
            .Index()
            .SelectMany(l => l.Value.Index().Select(q => (x: q.Key, y: l.Key, dir: q.Value)))
            .Where(b => b.dir is '<' or '>' or 'v' or '^')
            .ToLookup(key => (key.x, key.y), val => val.dir);
        
        var startY = 0;
        var startX = map[startY].IndexOf('.');
        var goalY = map.Length - 1;
        var goalX = map[goalY].IndexOf('.');
        
        return CalcFastestRoute(startX, startY, goalX, goalY, ref blizzards, map) + 1 +
               CalcFastestRoute(goalX, goalY, startX, startY, ref blizzards, map) + 1 +
               CalcFastestRoute(startX, startY, goalX, goalY, ref blizzards, map);
    }
    
    
    private static int CalcFastestRoute(int startX, int startY, int goalX, int goalY, ref ILookup<(int x, int y), char> blizzards, string[] map)
    {
        int blizzardsTime = 0;
        var directions = new[]
        {
            (dx: 0, dy: 0),
            (dx: 1, dy: 0),
            (dx: 0, dy: 1),
            (dx: -1, dy: 0),
            (dx: 0, dy: -1)
        };
        var blizzardMoves = new Dictionary<char, (int dx, int dy, int restartDx, int restartDy)>
        {
            { '>', (1, 0, -map[0].Length + 2, 0) },
            { '<', (-1, 0, map[0].Length - 2, 0) },
            { '^', (0, -1, 0, map.Length - 2) },
            { 'v', (0, 1, 0, -map.Length + 2) }
        };

        
        var visited = new HashSet<(int x, int y, int time)>();
        var searchQueue = new Queue<(int x, int y, int time)>();
        searchQueue.Enqueue((startX, startY, 0));
        while (searchQueue.Count > 0)
        {
            var state = searchQueue.Dequeue();
            if (state.x == goalX && state.y == goalY)
                return state.time;

            state.time++;
            if (blizzardsTime < state.time)
            {
                blizzards = blizzards.SelectMany(b => b.Select(b2 => (pos: b.Key, dir: b2)))
                    .Select(b => (b.pos, move: blizzardMoves[b.dir], dir: b.dir))
                    .Select(b => (x: b.pos.x + b.move.dx, y: b.pos.y + b.move.dy, b.move, b.dir))
                    .Select(b => map[b.y][b.x] == '#' ? (x: b.x + b.move.restartDx, y: b.y + b.move.restartDy, b.dir) : (b.x, b.y, b.dir))
                    .ToLookup(key => (key.x, key.y), val => val.dir);

                blizzardsTime++;
            }

            var blizzardsNoRef = blizzards;
            foreach (var s in directions.Select(d => (x: state.x + d.dx, y: state.y + d.dy)).Where(pos => pos.y >= 0 && pos.y < map.Length && map[pos.y][pos.x] != '#' && blizzardsNoRef.Contains(pos) == false))
                if (visited.Add((s.x, s.y, state.time)))
                    searchQueue.Enqueue((s.x, s.y, state.time));
        }

        throw new Exception("No solution found");
    }

    private static void Print(string[] map, ILookup<(int x, int y), char> blizzards)
    {
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (blizzards.Contains((x, y)))
                {
                    var numBlizzardsAtPoint = blizzards[(x,y)].Count();
                    if(numBlizzardsAtPoint == 1)
                        Console.Write(blizzards[(x, y)].First());
                    else
                        Console.Write(numBlizzardsAtPoint % 10);
                }
                else if(map[y][x] == '#')
                    Console.Write('#');
                else
                    Console.Write('.');
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine();
    }
}