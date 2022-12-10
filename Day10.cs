class Day10
{
    public object A()
    {
        var xChanges = File.ReadLines("Day10.txt")
            .SelectMany(l => l.Split(" "))
            .Select(l => int.TryParse(l, out var parsed) ? parsed : 0);

        int clock = 1;
        long score = 0;
        int x = 1;
        foreach (var cmd in xChanges)
        {
            if (clock >= 20 && (clock - 20) % 40 == 0)
                score += clock * x;

            x += cmd;
            clock++;
        }

        return score;
    }

    public object B()
    {
        var xChanges = File.ReadLines("Day10.txt")
            .SelectMany(l => l.Split(" "))  // addx takes two cycles and becomes two rows
            .Select(l => int.TryParse(l, out var parsed) ? parsed : 0); // addx, noop => 0

        int clock = 1;
        int x = 1;
        foreach (var cmd in xChanges)
        {
            var pos = clock % 40;
            if (pos == 1)
                Console.WriteLine();

            char chr = pos >= x && pos <= x + 2 ? '#' : '.';
            Console.Write(chr);
            
            x += cmd;
            clock++;
        }
        
        Console.WriteLine();
        return "ZKJFBJFZ";
    }
}