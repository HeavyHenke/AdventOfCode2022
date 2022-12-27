using System.Text.RegularExpressions;

class Day21
{
    public object A()
    {
        var monkeys = File.ReadLines("Day21.txt")
            .Select(m => m.Split(": "))
            .ToDictionary(key => key[0], val => val[1]);
        
        long GetValue(string monkeyName)
        {
            var m = monkeys[monkeyName];
            if (long.TryParse(m, out var val))
                return val;

            var match = Regex.Match(m, @"(?<m1>\w+) (?<op>[-\/\*\+]) (?<m2>\w+)");
            var v1 = GetValue(match.Groups["m1"].Value);
            var v2 = GetValue(match.Groups["m2"].Value);

            if (match.Groups["op"].Value == "+")
                return v1 + v2;
            if (match.Groups["op"].Value == "-")
                return v1 - v2;
            if (match.Groups["op"].Value == "*")
                return v1 * v2;
            if (match.Groups["op"].Value == "/")
                return v1 / v2;
            throw new Exception("Knas");
        }
        
        return GetValue("root");
    }

    public object B()
    {
        var monkeys = File.ReadLines("Day21.txt")
            .Select(m => m.Split(": "))
            .ToDictionary(key => key[0], val => val[1]);

        var root = monkeys["root"];
        var match = Regex.Match(root, @"(?<m1>\w+) (?<op>[-\/\*\+]) (?<m2>\w+)");
        monkeys["root"] = $"{match.Groups["m1"].Value} - {match.Groups["m2"].Value}";

        long humanValue = 0;
        
        double GetValue(string monkeyName)
        {
            if (monkeyName == "humn")
                return humanValue;
            
            var m = monkeys[monkeyName];
            if (long.TryParse(m, out var val))
                return val;

            var match = Regex.Match(m, @"(?<m1>\w+) (?<op>[-\/\*\+]) (?<m2>\w+)");
            var v1 = GetValue(match.Groups["m1"].Value);
            var v2 = GetValue(match.Groups["m2"].Value);

            if (match.Groups["op"].Value == "+")
                return v1 + v2;
            if (match.Groups["op"].Value == "-")
                return v1 - v2;
            if (match.Groups["op"].Value == "*")
                return v1 * v2;
            if (match.Groups["op"].Value == "/")
                return v1 / v2;
            throw new Exception("Knas");
        }
        
        
        long bestPosX;
        long bestNegX;

        humanValue = long.MaxValue;
        var value = GetValue("root");
        if (value > 0)
        {
            bestPosX = long.MaxValue;
            bestNegX = long.MinValue;
        }
        else
        {
            bestPosX = long.MinValue;
            bestNegX = long.MaxValue;
        }

        while (true)
        {
            humanValue = (bestNegX + bestPosX + 1) / 2;
            var val = GetValue("root");
            if (val == 0)
                return humanValue;
            if (val > 0) 
                bestPosX = humanValue;
            else 
                bestNegX = humanValue;
        }
    }
}