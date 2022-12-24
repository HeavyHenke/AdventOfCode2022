
class Day17
{
    public object A()
    {
        var movement = File.ReadAllText("Day17.txt");

        var map = new HashSet<(int x, int y)>();
        int highestPoint = -1;
        
        var shape1 = new (int x, int y)[] { (2, 0), (3, 0), (4, 0), (5, 0) };
        var shape2 = new (int x, int y)[] { (2, 1), (3, 0), (3, 1), (3, 2), (4, 1) };
        var shape3 = new (int x, int y)[] { (2, 0), (3, 0), (4, 0), (4, 1), (4, 2) };
        var shape4 = new (int x, int y)[] { (2, 0), (2, 1), (2, 2), (2, 3) };
        var shape5 = new (int x, int y)[] { (2, 0), (2, 1), (3, 0), (3, 1) };
        var shapes = new[] { shape1, shape2, shape3, shape4, shape5 };
        int shapeIx = 0;
        int gasIx = 0;


        for (int i = 0; i < 2022; i++)
        {
            var fallingRock = shapes[shapeIx++].Select(r => (r.x, r.y + highestPoint + 4)).ToArray();
            if (shapeIx >= shapes.Length)
                shapeIx = 0;

            while (true)
            {
                // Is it pushable?
                int dx = movement[gasIx++] == '>' ? 1 : -1;
                if (gasIx >= movement.Length)
                    gasIx = 0;

                var pushedRock = fallingRock.Select(r => (r.Item1 + dx, r.Item2)).ToArray();
                if (pushedRock.All(r => map.Contains(r) == false && r.Item1 is >= 0 and < 7))
                    fallingRock = pushedRock;
                
                // can it fall? false update map+break
                var rockThatHasFallen = fallingRock.Select(r => (r.Item1, r.Item2 - 1)).ToArray();
                if (rockThatHasFallen.All(r => map.Contains(r) == false && r.Item2 >= 0))
                {
                    fallingRock = rockThatHasFallen;
                }
                else
                {
                    foreach (var r in fallingRock)
                    {
                        map.Add(r);
                        highestPoint = Math.Max(highestPoint, r.Item2);
                    }
                    break;
                }
            }
        }

        return highestPoint + 1;
    }
    
    public object B()
    {
        var movement = File.ReadAllText("Day17_test.txt");

        var map = new List<byte>();
        int highestPoint = -1;

        var shape1 = new byte[] { (1 << 2) + (1 << 3) + (1 << 4) + (1 << 5) };
        var shape2 = new byte[] { (1 << 3), (1 << 2) + (1 << 3) + (1 << 4), (1 << 3) };
        var shape3 = new byte[] { (1 << 2) + (1 << 3) + (1 << 4), (1 << 4), (1 << 4) };
        var shape4 = new byte[] { (1 << 2), (1 << 2), (1 << 2), (1 << 2) };
        var shape5 = new byte[] { (1 << 2) + (1 << 3), (1 << 2) + (1 << 3) };
        var shapes = new[] { shape1, shape2, shape3, shape4, shape5 };
        int shapeIx = 0;
        int gasIx = 0;
        int purgedRows = 0;


        var lastMapAtGas1 = map.ToList();
        var lastIxAtGat1 = 0;

        // 19,5 dagar
        //                   1000000000000
        for (long i = 0; i < 2022; i++)
        {
            var rockPos = highestPoint + 4;
            var fallingRock = shapes[shapeIx++].ToArray();
            if (shapeIx >= shapes.Length)
            {
                shapeIx = 0;
                if (gasIx == 0 && map.Count == lastMapAtGas1.Count && map.SequenceEqual(lastMapAtGas1))
                {
                    Console.WriteLine("Samma!")
                    ;
                }
            }

            while (true)
            {
                // Is it pushable?
                int dx = movement[gasIx++] == '>' ? 1 : -1;
                if (gasIx >= movement.Length)
                {
                    gasIx = 0;
                }

                // Can be pushed?
                byte[] pushedRock;
                if (dx == 1)
                    pushedRock = fallingRock.Select(r => (byte)(r << 1)).ToArray();
                else
                    pushedRock = fallingRock.Select(r => (byte)(r >> 1)).ToArray();
                
                bool canBePushed = true;
                for (int rocki = 0; rocki < fallingRock.Length && canBePushed; rocki++)
                {
                    if ((dx == 1 && (fallingRock[rocki] & 64) != 0) || (dx == -1 && (fallingRock[rocki] & 1) != 0))
                    {
                        // Check wall collision
                        canBePushed = false;
                        break;
                    }
                    
                    int mapY = rockPos + rocki;
                    if (mapY < map.Count && (map[mapY] & pushedRock[rocki]) != 0)
                        canBePushed = false;
                }
                if (canBePushed)
                {
                    fallingRock = pushedRock;
                }

                // Fall down if possible
                bool canFall = true;
                for (int rocki = 0; rocki < fallingRock.Length && canFall; rocki++)
                {
                    var mapY = rocki + rockPos - 1;
                    if (mapY >= map.Count)
                        break;
                    if (mapY < 0)
                        canFall = false;
                    else if ((map[mapY] & fallingRock[rocki]) != 0)
                        canFall = false;
                }

                if (canFall)
                {
                    rockPos--;
                }
                else
                {
                    for (int rocki = 0; rocki < fallingRock.Length; rocki++)
                    {
                        var mapY = rocki + rockPos;
                        if(mapY >= map.Count)
                            map.Add(0);
                        map[mapY] |= fallingRock[rocki];
                        highestPoint = Math.Max(highestPoint, mapY);

                        if (map[mapY] == 0x7F)  // Purge on full line
                        {
                            var rowsToRemove = mapY + 1;
                            purgedRows += rowsToRemove;
                            rockPos -= rowsToRemove;
                            highestPoint -= rowsToRemove;
                            map.RemoveRange(0, rowsToRemove);
                        }
                    }

                    // Draw the map
                    // var lines = new List<string>();
                    // for (int y = 0; y < map.Count; y++)
                    // {
                    //     var line = new string(Convert.ToString(map[y], 2).PadLeft(7, '0').Reverse().ToArray());
                    //     line = line.Replace('1', '#').Replace('0', '.');
                    //     lines.Add(line);
                    // }
                    //
                    // lines.Reverse();
                    // foreach (var line in lines)
                    //     Console.WriteLine(line);
                    // Console.WriteLine();
                    
                    break;
                }
            }
        }

        return highestPoint + 1 + purgedRows;
    }

}