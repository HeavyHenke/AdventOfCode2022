class Day9
{
    public object A()
    {
        var headPos = (0, 0);
        var tailPos = (0, 0);
        var tailVisited = new HashSet<(int x, int y)> { tailPos };

        var commands = File.ReadLines("Day9.txt")
            .Select(l => l.Split(' '))
            .Select(l => (direction: l[0][0], count: int.Parse(l[1])));

        foreach (var cmd in commands)
        {
            for (int c = 0; c < cmd.count; c++)
            {
                headPos = MoveHead(cmd.direction, headPos);
                tailPos = MoveTail(headPos, tailPos);
                tailVisited.Add(tailPos);
            }
        }
        
        return tailVisited.Count;
    }

    public object B()
    {
        var positions = Enumerable.Repeat((x: 0, y: 0), 10).ToArray();
        var tailVisited = new HashSet<(int x, int y)> { positions.Last() };

        var commands = File.ReadLines("Day9.txt")
            .Select(l => l.Split(' '))
            .Select(l => (direction: l[0][0], count: int.Parse(l[1])));

        foreach (var cmd in commands)
        {
            for (int c = 0; c < cmd.count; c++)
            {
                positions[0] = MoveHead(cmd.direction, positions[0]);
                for (int i = 1; i < positions.Length; i++)
                    positions[i] = MoveTail(positions[i - 1], positions[i]);
                tailVisited.Add(positions.Last());
            }
        }
        
        return tailVisited.Count;

    }

    private static (int x, int y) MoveTail((int x, int y) head, (int x, int y) tail)
    {
        if (head.x == tail.x && Math.Abs(head.y - tail.y) >= 2)
            return (tail.x, tail.y + Math.Sign(head.y - tail.y));
        if (head.y == tail.y && Math.Abs(head.x - tail.x) >= 2)
            return (tail.x + Math.Sign(head.x - tail.x), tail.y);

        bool isTouching = Math.Abs(head.x - tail.x) <= 1 && Math.Abs(head.y - tail.y) <= 1;
        if (!isTouching)
            return (tail.x + Math.Sign(head.x - tail.x), tail.y + Math.Sign(head.y - tail.y));

        return tail;
    }

    private static (int x, int y) MoveHead(char direction, (int x, int y) headPos)
    {
        return direction switch
        {
            'U' => (headPos.x, headPos.y - 1),
            'D' => (headPos.x, headPos.y + 1),
            'L' => (headPos.x - 1, headPos.y),
            'R' => (headPos.x + 1, headPos.y),
            _ => throw new ArgumentException("Direction: " + direction)
        };
    }
    
}