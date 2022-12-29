using AdventOfCode2022;

DateTime start = DateTime.Now;
string result = new Day25().A()?.ToString() ?? " ";
DateTime stop = DateTime.Now;

Console.WriteLine("It took " + (stop - start).TotalSeconds);

WindowsClipboard.SetText(result);
Console.WriteLine(result);