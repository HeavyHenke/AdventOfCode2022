// See https://aka.ms/new-console-template for more information


using AdventOfCode2022;

DateTime start = DateTime.Now;
string result = new Day11().A()?.ToString() ?? " ";
DateTime stop = DateTime.Now;

Console.WriteLine("It took " + (stop - start).TotalSeconds);

WindowsClipboard.SetText(result);
Console.WriteLine(result);

class Day11
{
    public object A()
    {
        return 0;
    }
}
