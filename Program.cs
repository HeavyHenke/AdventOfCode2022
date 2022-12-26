// See https://aka.ms/new-console-template for more information


using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using AdventOfCode2022;

DateTime start = DateTime.Now;
string result = new Day19().B()?.ToString() ?? " ";
DateTime stop = DateTime.Now;

Console.WriteLine("It took " + (stop - start).TotalSeconds);

WindowsClipboard.SetText(result);
Console.WriteLine(result);

class Day20
{
    public object A()
    {
        return 0;
    }
}