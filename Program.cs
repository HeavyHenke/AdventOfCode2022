// See https://aka.ms/new-console-template for more information


using AdventOfCode2022;

DateTime start = DateTime.Now;
string result = new Day9().B()?.ToString() ?? " ";
DateTime stop = DateTime.Now;

Console.WriteLine("It took " + (stop - start).TotalSeconds);

WindowsClipboard.SetText(result);
Console.WriteLine(result);