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
    
}