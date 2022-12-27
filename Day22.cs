using System.Buffers.Text;

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

    private static void Print(string[] map, Dictionary<(int x, int y),int> visted)
    {
        Console.WriteLine();
        var dirChars = new[] { '>', 'v', '<', '^' };
        var v2 = visted.ToDictionary(key => key.Key, val => val.Value);
        for (int y = 0; y < map.Length; y++)
        {
            for (int x = 0; x < map[y].Length; x++)
            {
                if (v2.TryGetValue((x, y), out var dir))
                {
                    Console.Write(dirChars[dir]);
                    v2.Remove((x, y));
                }
                else
                {
                    Console.Write(map[y][x]);
                }
            }

            Console.WriteLine();
        }

        if (v2.Any())
            throw new Exception("Knas");
    }
}