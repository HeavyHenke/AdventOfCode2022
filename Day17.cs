
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
}