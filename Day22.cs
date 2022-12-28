using System.Buffers.Text;
using MoreLinq.Extensions;

class Day22
{
    public object A()
    {
        var lines = File.ReadAllLines("Day22.txt");
        var map = lines.TakeWhile(l => l != "").ToArray();
        var directions = lines.Last().Select(c => (byte)c).ToArray();
        var visted = new Dictionary<(int x, int y), int>();
        int directionIndex = 0;

        var dir = 0;
        int x = map[0].IndexOf('.');
        int y = 0;
        
        while (directionIndex < directions.Length)
        {
            if (directions[directionIndex] == 'R')
            {
                dir++;
                if (dir == 4)
                    dir = 0;
                directionIndex++;
                continue;
            }
            if (directions[directionIndex] == 'L')
            {
                dir--;
                if (dir == -1)
                    dir = 3;
                directionIndex++;
                continue;
            }

            var b = Utf8Parser.TryParse(new ReadOnlySpan<byte>(directions, directionIndex, directions.Length - directionIndex), out int steps, out int bytesRead);
            if (b == false)
                throw new Exception("knas");
            directionIndex += bytesRead;

            while (steps > 0)
            {
                int nx = x, ny = y;
                switch (dir)
                {
                    case 0:
                    {
                        nx++;
                        if (nx >= map[ny].Length || map[ny][nx] == ' ') 
                            nx = map[ny].IndexOfAny(new[] { '.', '#' });
                        break;
                    }
                    case 2:
                    {
                        nx--;
                        if (nx < 0 || map[ny][nx] == ' ') 
                            nx = map[ny].LastIndexOfAny(new[] { '.', '#' });
                        break;
                    }
                    case 1:
                    {
                        do
                        {
                            ny++;
                            if (ny >= map.Length)
                                ny = 0;
                        } while (nx >= map[ny].Length || map[ny][nx] == ' ');

                        break;
                    }
                    case 3:
                    {
                        do
                        {
                            ny--;
                            if (ny < 0)
                                ny = map.Length - 1;
                        } while (nx >= map[ny].Length || map[ny][nx] == ' ');

                        break;
                    }
                }

                if (map[ny][nx] == '#')
                    break;

                x = nx;
                y = ny;
                visted[(x, y)] = dir;

                steps--;
            }
        }
        
        // Print(map, visted);

        x++;
        y++;
        return y * 1000L + x * 4 + dir;
    }
    
    public object B()
    {
        const int sideLength = 50;
        var lines = File.ReadAllLines("Day22.txt");
        
        const int maxCoord = sideLength - 1;

        var map = lines.TakeWhile(l => l != "")
            .Index()
            .SelectMany(l => l.Value.Index().Select(c => (y: l.Key, x: c.Key, val: c.Value)))
            .Where(c => c.val is '.' or '#')
            .ToDictionary(key => (key.x, key.y), val => val.val);
        
        var directions = lines.Last().Select(c => (byte)c).ToArray();
        int directionIndex = 0;
        
        var visted = new Dictionary<(int x, int y), int>();
        var testedWraps = new HashSet<int>{0};

        var dir = 0;
        int x = lines[0].IndexOf('.');
        int y = 0;

        while (directionIndex < directions.Length)
        {
            if (directions[directionIndex] == 'R')
            {
                dir++;
                if (dir == 4)
                    dir = 0;
                directionIndex++;
                continue;
            }

            if (directions[directionIndex] == 'L')
            {
                dir--;
                if (dir == -1)
                    dir = 3;
                directionIndex++;
                continue;
            }

            var b = Utf8Parser.TryParse(new ReadOnlySpan<byte>(directions, directionIndex, directions.Length - directionIndex), out int steps, out int bytesRead);
            if (b == false)
                throw new Exception("knas");
            directionIndex += bytesRead;
            
            while (steps > 0)
            {
                int nx = x, ny = y;
                int ndir = dir;
                int wrapNum = 0;
                switch (dir)
                {
                    case 0:
                    {
                        nx++;
                        if (map.ContainsKey((nx, ny)) == false)
                        {
                            if (ny < sideLength)
                            {
                                wrapNum = 1;
                                ndir = 2;
                                nx = sideLength + maxCoord;
                                ny = 2 * sideLength + maxCoord - y;
                            }
                            else if (ny < 2 * sideLength)
                            {
                                wrapNum = 2;
                                ndir = 3;
                                nx = 2 * sideLength + y % sideLength;
                                ny = maxCoord;
                            }
                            else if (ny < 3 * sideLength)
                            {
                                wrapNum = 3;
                                ndir = 2;
                                nx = 2 * sideLength + maxCoord;
                                ny = maxCoord - (y - 2 * sideLength);
                            }
                            else
                            {
                                wrapNum = 4;
                                ndir = 3;
                                nx = sideLength + (y - 3 * sideLength);
                                ny = 2 * sideLength + maxCoord;
                            }
                        }
                        break;
                    }
                    case 2:
                    {
                        nx--;
                        if (map.ContainsKey((nx, ny)) == false)
                        {
                            if (ny < sideLength)
                            {
                                wrapNum = 5;
                                ndir = 0;
                                nx = 0;
                                ny = 2 * sideLength + maxCoord - y;
                            }
                            else if (ny < 2 * sideLength)
                            {
                                wrapNum = 6;
                                ndir = 1;
                                nx = y - sideLength;
                                ny = 2 * sideLength;
                            }
                            else if (ny < 3 * sideLength)
                            {
                                wrapNum = 7;
                                ndir = 0;
                                nx = sideLength;
                                ny = maxCoord - (y % sideLength);
                            }
                            else
                            {
                                wrapNum = 8;
                                ndir = 1;
                                nx = sideLength + y % sideLength;
                                ny = 0;
                            }
                        }
                        break;
                    }
                    case 1:
                    {
                        ny++;
                        if (map.ContainsKey((nx, ny)) == false)
                        {
                            if (x < sideLength)
                            {
                                wrapNum = 9;
                                ndir = 1;
                                nx = 2*sideLength + x%sideLength;
                                ny = 0;
                            }
                            else if (x < 2 * sideLength)
                            {
                                wrapNum = 10;
                                ndir = 2;
                                nx = maxCoord;
                                ny = 3 * sideLength + x % sideLength;
                            }
                            else
                            {
                                wrapNum = 11;
                                ndir = 2;
                                nx = sideLength + maxCoord;
                                ny = sideLength + x % sideLength;
                            }
                        }
                        break;
                    }
                    case 3:
                    {
                        ny--;
                        if (map.ContainsKey((nx, ny)) == false)
                        {
                            if (x < sideLength)
                            {
                                wrapNum = 12;
                                ndir = 0;
                                nx = sideLength;
                                ny = sideLength + x % sideLength;
                            }
                            else if (x < 2 * sideLength)
                            {
                                wrapNum = 13;
                                ndir = 0;
                                nx = 0;
                                ny = 3 * sideLength + x % sideLength;
                            }
                            else
                            {
                                wrapNum = 14;
                                ndir = 3;
                                nx = x % sideLength;
                                ny = 3 * sideLength + maxCoord;
                            }
                        }
                        break;
                    }
                }

                if (map[(nx, ny)] == '#')
                    break;

                if (testedWraps.Add(wrapNum))
                {
                   // Print(map, visted, sideLength, x, y, dir, nx, ny, ndir);
                }

                x = nx;
                y = ny;
                dir = ndir;
                visted[(x, y)] = dir;
                
                steps--;
            }
        }
        
        x++;
        y++;
        return y * 1000L + x * 4 + dir;
    }

    private static void Print(Dictionary<(int x, int y), char> map, Dictionary<(int x, int y), int> visted, int side, int markX, int markY, int markDir, int markX2, int markY2, int markDir2)
    {
        Console.WriteLine();
        var color = Console.BackgroundColor;
        var dirChars = new[] { '>', 'v', '<', '^' };
        for (int y = 0; y < 4 * side; y++)
        {
            for (int x = 0; x < 4 * side; x++)
            {
                if (x == markX && y == markY)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(dirChars[markDir]);
                    Console.BackgroundColor = color;
                }
                else if (x == markX2 && y == markY2)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(dirChars[markDir2]);
                    Console.BackgroundColor = color;
                }
                else if (visted.TryGetValue((x, y), out var dir))
                    Console.Write(dirChars[dir]);
                else if (map.TryGetValue((x, y), out var chr))
                    Console.Write(chr);
                else
                    Console.Write(' ');
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}