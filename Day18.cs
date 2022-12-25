class Day18
{
    public object A()
    {
        _cubes = File.ReadLines("Day18.txt")
            .Select(l => l.Split(','))
            .Select(l => (x: int.Parse(l[0]), y: int.Parse(l[1]), z: int.Parse(l[2])))
            .ToHashSet();

        var numOpenSides = _cubes
            .SelectMany(c => _directions.Select(d => (c.x + d.dx, c.y + d.dy, c.z + d.dz)))
            .Count(c => _cubes.Contains(c) == false);
        
        return numOpenSides;
    }
    
    public object B()
    {
        _cubes = File.ReadLines("Day18.txt")
            .Select(l => l.Split(','))
            .Select(l => (x: int.Parse(l[0]), y: int.Parse(l[1]), z: int.Parse(l[2])))
            .ToHashSet();

        _minX = _cubes.Min(c => c.x);
        _maxX = _cubes.Max(c => c.x);
        _minY = _cubes.Min(c => c.y);
        _maxY = _cubes.Max(c => c.y);
        _minZ = _cubes.Min(c => c.z);
        _maxZ = _cubes.Max(c => c.z);
        
        var numOpenSides = _cubes
            .SelectMany(c => _directions.Select(d => (c.x + d.dx, c.y + d.dy, c.z + d.dz)))
            .Count(c => IsContained(c) == false);
        
        return numOpenSides;
    }

    private int _minX, _maxX, _minY, _maxY, _minZ, _maxZ;
    private HashSet<(int x, int y, int z)> _cubes;
    private readonly Dictionary<(int x, int y, int z), bool> _knownClosed = new();
    private readonly (int dx, int dy, int dz)[] _directions = { (-1, 0, 0), (1, 0, 0), (0, -1, 0), (0, 1, 0), (0, 0, -1), (0, 0, 1) };

    private bool IsContained((int x, int y, int z) pos)
    {
        if (_cubes.Contains(pos))
            return true;

        if (_knownClosed.TryGetValue(pos, out var contained))
            return contained;
        if (pos.x < _minX || pos.x > _maxX || pos.y < _minY || pos.y > _maxY || pos.z < _minZ || pos.z > _maxZ)
            return false;

        var visited = new HashSet<(int x, int y, int z)>();
        var search = new Stack<(int x, int y, int z)>();
        visited.Add(pos);
        search.Push(pos);

        while (search.Count > 0)
        {
            pos = search.Pop();

            var foundInKnown = _knownClosed.TryGetValue(pos, out var closed);
            if (foundInKnown && closed)
            {
                foreach(var p in visited)
                    _knownClosed[p] = true;
                return true;
            }

            if ((foundInKnown && !closed) || (pos.x < _minX || pos.x > _maxX || pos.y < _minY || pos.y > _maxY || pos.z < _minZ || pos.z > _maxZ))
            {
                foreach(var p in visited)
                    _knownClosed[p] = false;
                return false;
            }

            var next = _directions.Select(d => (pos.x + d.dx, pos.y + d.dy, pos.z + d.dz));
            foreach (var p in next)
            {
                if(_cubes.Contains(p) == false)
                    if(visited.Add(p))
                        search.Push(p);
            }
        }

        foreach(var p in visited)
            _knownClosed[p] = true;
        return true;
    }
}