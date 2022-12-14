using MoreLinq;

class Day14
{
    public object A()
    {
        var rocks = File.ReadLines("Day14.txt")
            .Select(l =>
                l.Split(" -> ").Select(coord => coord.Split(','))
                    .Select(c => (x: int.Parse(c[0]), y: int.Parse(c[1]))).ToList());

        var map = new Dictionary<(int x, int y), char>();
        foreach (var r in rocks)
        {
            for (var i = 0; i < r.Count - 1; i++)
            {
                var rockPoints = MoreEnumerable.Sequence(r[i].x, r[i + 1].x).Cartesian(MoreEnumerable.Sequence(r[i].y, r[i + 1].y), (cx, cy) => (cx, cy));
                foreach (var pt in rockPoints)
                    map[pt] = '#';
            }
        }

        var largestY = map.Keys.Max(k => k.y);
        bool overFlow = false;
        while (!overFlow)
        {
            var sand = (x: 500, y: 0);
            while (true)
            {
                if (map.ContainsKey((sand.x, sand.y + 1)) == false)
                {
                    sand = (sand.x, sand.y + 1);
                }
                else if (map.ContainsKey((sand.x - 1, sand.y + 1)) == false)
                {
                    sand = (sand.x-1, sand.y + 1);
                }
                else if (map.ContainsKey((sand.x + 1, sand.y + 1)) == false)
                {
                    sand = (sand.x+1, sand.y + 1);
                }
                else
                {
                    map.Add(sand, 'O');
                    break;
                }

                if (sand.y > largestY)
                {
                    overFlow = true;
                    break;
                }
            }
        }

        return map.Values.Count(v => v == 'O');
    }

    public object B()
    {
        var rocks = File.ReadLines("Day14.txt")
            .Select(l =>
                l.Split(" -> ").Select(coord => coord.Split(','))
                    .Select(c => (x: int.Parse(c[0]), y: int.Parse(c[1]))).ToList());

        var map = new Dictionary<(int x, int y), char>();
         foreach (var r in rocks)
         {
             for (var i = 0; i < r.Count - 1; i++)
             {
                 var rockPoints = MoreEnumerable.Sequence(r[i].x, r[i + 1].x)
                     .Cartesian(MoreEnumerable.Sequence(r[i].y, r[i + 1].y), (cx, cy) => (cx, cy));
                 foreach (var pt in rockPoints)
                     map[pt] = '#';
             }
         }

        var floor = map.Keys.Max(k => k.y) + 1;
        bool overFlow = false;
        while (!overFlow)
        {
            var sand = (x: 500, y: 0);
            while (true)
            {
                if (sand.y == floor)
                {
                    map.Add(sand, 'O');
                    break;
                }

                if (map.ContainsKey((sand.x, sand.y + 1)) == false)
                {
                    sand = (sand.x, sand.y + 1);
                }
                else if (map.ContainsKey((sand.x - 1, sand.y + 1)) == false)
                {
                    sand = (sand.x - 1, sand.y + 1);
                }
                else if (map.ContainsKey((sand.x + 1, sand.y + 1)) == false)
                {
                    sand = (sand.x + 1, sand.y + 1);
                }
                else
                {
                    map.Add(sand, 'O');
                    if (sand == (500, 0))
                        overFlow = true;
                    break;
                }
            }
        }

        return map.Values.Count(v => v == 'O');
    }
}
