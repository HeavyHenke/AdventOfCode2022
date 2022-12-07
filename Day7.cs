class Day7
{
    public object A()
    {
        var root = Parse("Day7.txt");
        return root.GetAllDirsWithMaxSize().Sum(d => d.CalcSize());
    }

    private static Directory Parse(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        string currentDir = "/";
        Directory root = new Directory("/");
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("$ cd"))
            {
                var cdArg = lines[i].Split(' ').Last();
                if (cdArg.StartsWith("/"))
                    currentDir = cdArg;
                else if (cdArg == "..")
                    currentDir = string.Join("/", currentDir.Split("/").SkipLast(1));
                else if (currentDir.EndsWith("/"))
                    currentDir += cdArg;
                else
                    currentDir += "/" + cdArg;
            }
            else if (lines[i].StartsWith("$ ls"))
            {
                while (i + 1 < lines.Length && lines[i + 1].StartsWith("$") == false)
                {
                    var l = lines[++i];
                    var lineParts = l.Split(" ");
                    var dir = root.GetDir(currentDir);
                    if (lineParts[0] == "dir")
                        dir.SubDirs.Add(new Directory(lineParts[1]));
                    else
                        dir.Files.Add((lineParts[1], long.Parse(lineParts[0])));
                }
            }
        }

        return root;
    }

    public object B()
    {
        var root = Parse("Day7.txt");
        long unusedSpace = 70000000 - root.CalcSize();
        return root.GetAllDirs()
            .Where(d => d.CalcSize() >= 30000000 - unusedSpace)
            .OrderBy(s => s.CalcSize())
            .First()
            .CalcSize();
    }
    
    class Directory
    {
        public string Name { get; }
        public List<Directory> SubDirs { get; } = new();
        public List<(string name, long size)> Files { get; } = new();
        private long? _size;
        
        
        public Directory(string name)
        {
            Name = name;
        }
        
        public Directory GetDir(string path)
        {
            if (path == "/" || string.IsNullOrEmpty(path))
                return this;
            var pathParts = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
            var subDirName = pathParts.First();
            var subDir = SubDirs.First(d => d.Name == subDirName);
            return subDir.GetDir(string.Join("/", pathParts.Skip(1)));
        }

        public long CalcSize()
        {
            _size ??= Files.Sum(f => f.size) + SubDirs.Sum(d => d.CalcSize());
            return _size.Value;
        }

        public IEnumerable<Directory> GetAllDirsWithMaxSize(long maxSize = 100000)
        {
            var ret = SubDirs.Where(d => d.CalcSize() <= maxSize)
                .Concat(SubDirs.SelectMany(d => d.GetAllDirsWithMaxSize(maxSize)));
            return ret;
        }

        public IEnumerable<Directory> GetAllDirs()
        {
            return SubDirs.Concat(SubDirs.SelectMany(s => s.GetAllDirs()));
        }
    }
}