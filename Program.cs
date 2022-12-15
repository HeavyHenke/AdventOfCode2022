﻿// See https://aka.ms/new-console-template for more information


using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using AdventOfCode2022;
using MoreLinq;

DateTime start = DateTime.Now;
string result = new Day15().B()?.ToString() ?? " ";
DateTime stop = DateTime.Now;

Console.WriteLine("It took " + (stop - start).TotalSeconds);

WindowsClipboard.SetText(result);
Console.WriteLine(result);


class Day15
{
    public object A()
    {
        var beacons = File.ReadLines("Day15.txt")
            .Select(r => Regex.Match(r, @"^Sensor at x=(?<sx>-?\d+), y=(?<sy>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)$"))
            .Select(r => (bx: int.Parse( r.Groups["bx"].Value), by: int.Parse(r.Groups["by"].Value), sx: int.Parse(r.Groups["sx"].Value), sy: int.Parse(r.Groups["sy"].Value)))
            .Select(r => (x: r.sx, y: r.sy, manhattan: Math.Abs(r.bx-r.sx)+Math.Abs(r.by-r.sy), r.bx, r.by))
            .ToList();

        var xStart = beacons.Min(b => b.x - b.manhattan) - 1;
        var xStop = beacons.Max(b => b.x + b.manhattan) + 1;
        int y = 2000000;
        
        int numImpossible = 0;
        for (int x = xStart; x <= xStop; x++)
        {
            foreach (var b in beacons)
            {
                if (x == b.bx && y == b.by)
                    break;  // There is a beacon exactly here so not impossible
                var dist = Math.Abs(x - b.x) + Math.Abs(y - b.y);
                if (dist <= b.manhattan)
                {
                    numImpossible++;
                    break;
                }
            }
        }

        return numImpossible;
    }
    
    public object B()
    {
        var beacons = File.ReadLines("Day15.txt")
            .Select(r => Regex.Match(r, @"^Sensor at x=(?<sx>-?\d+), y=(?<sy>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)$"))
            .Select(r => (bx: int.Parse( r.Groups["bx"].Value), by: int.Parse(r.Groups["by"].Value), sx: int.Parse(r.Groups["sx"].Value), sy: int.Parse(r.Groups["sy"].Value)))
            .Select(r => (x: r.sx, y: r.sy, manhattan: Math.Abs(r.bx-r.sx)+Math.Abs(r.by-r.sy), r.bx, r.by))
            .ToList();
        
        int numImpossible = 0;
        for (int y = 0; y <= 4000000; y++)
        {
            // Console.WriteLine("Y: " + y);
            for (int x = 0; x <= 4000000; x++)
            {
                bool possible = true;
                foreach (var b in beacons)
                {
                    if (x == b.bx && y == b.by)
                    {
                        possible = false;
                        break; // There is a beacon exactly here so not impossible
                    }

                    var dist = Math.Abs(x - b.x) + Math.Abs(y - b.y);
                    if (dist <= b.manhattan)
                    {
                        x = b.x + b.manhattan - Math.Abs(y - b.y);
                        possible = false;
                        break;
                    }
                }

                if (possible)
                {
                    return x * 4000000L + y;
                }
            }
        }

        throw new Exception("not found");
    }

}
