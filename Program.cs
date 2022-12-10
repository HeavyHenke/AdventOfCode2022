// See https://aka.ms/new-console-template for more information


using AdventOfCode2022;

DateTime start = DateTime.Now;
string result = new Day10().B()?.ToString() ?? " ";
DateTime stop = DateTime.Now;

Console.WriteLine("It took " + (stop - start).TotalSeconds);

WindowsClipboard.SetText(result);
Console.WriteLine(result);


class Day10
{
    public object A()
    {
        var commands = File.ReadLines("Day10.txt")
            .Select(l => l.Split(" "))
            .ToList();

        int cmdIx = 0;
        int? nextAdd = null;
        long score = 0;
        int x = 1;
        for (int clock = 1; cmdIx < commands.Count; clock++)
        {
            if (clock >= 20 && (clock - 20) % 40 == 0)
                score += clock * x;
            
            if (nextAdd.HasValue)
            {
                x += nextAdd.Value;
                nextAdd = null;
                continue;
            }

            var cmd = commands[cmdIx++];
            if (cmd[0] == "addx")
            {
                nextAdd = int.Parse(cmd[1]);
            }
        }
        
        return score;
    }
    
    public object B()
    {
        var commands = File.ReadLines("Day10.txt")
            .Select(l => l.Split(" "))
            .ToList();

        int cmdIx = 0;
        int? nextAdd = null;
        long score = 0;
        int x = 1;
        for (int clock = 1; cmdIx < commands.Count; clock++)
        {
            var pos = clock % 40;
            if (pos == 1)
                Console.WriteLine();

            char chr = pos >= x && pos <= x + 2 ? '#' : '.';
            Console.Write(chr);
            
            if (nextAdd.HasValue)
            {
                x += nextAdd.Value;
                nextAdd = null;
                continue;
            }

            var cmd = commands[cmdIx++];
            if (cmd[0] == "addx")
            {
                nextAdd = int.Parse(cmd[1]);
            }
        }
        
        Console.WriteLine();
        return "ZKJFBJFZ";
    }

}