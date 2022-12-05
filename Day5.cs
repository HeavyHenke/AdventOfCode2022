using MoreLinq;

class Day5
{
    public object A()
    {
        var lines = File.ReadAllLines("Day5.txt");

        var stacks = lines.TakeWhile(l => !string.IsNullOrEmpty(l))
            .Select(l => l.Batch(4).Select(b => b.ToList()).TakeWhile(w => w.Count >= 2).Select(q => q[1]))
            .Transpose()
            .Select(l => new Stack<char>(l.Where(char.IsUpper).Reverse()))
            .ToArray();

        var commands = lines.SkipWhile(l => !l.StartsWith("move"))
            .Select(l => l.Split(new[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());

        foreach (var cmd in commands)
        {
            int times = cmd[0];
            while (times > 0)
            {
                var c = stacks[cmd[1]-1].Pop();
                stacks[cmd[2]-1].Push(c);
                times--;
            }
        }
        
        var message = stacks.Select(s => s.Pop());
        return new string(message.ToArray());
    }
    
    public object B()
    {
        var lines = File.ReadAllLines("Day5.txt");

        var stacks = lines.TakeWhile(l => !string.IsNullOrEmpty(l))
            .Select(l => l.Batch(4).Select(b => b.ToList()).TakeWhile(w => w.Count >= 2).Select(q => q[1]))
            .Transpose()
            .Select(l => new Stack<char>(l.Where(char.IsUpper).Reverse()))
            .ToArray();

        var commands = lines.SkipWhile(l => !l.StartsWith("move"))
            .Select(l => l.Split(new[] { "move ", " from ", " to " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray());

        foreach (var cmd in commands)
        {
            int times = cmd[0];
            var crane = new Stack<char>();
            while (times > 0)
            {
                crane.Push(stacks[cmd[1] - 1].Pop());
                times--;
            }
            while(crane.Any())
                stacks[cmd[2]-1].Push(crane.Pop());
        }
        
        var message = stacks.Select(s => s.Pop());
        return new string(message.ToArray());
    }
}