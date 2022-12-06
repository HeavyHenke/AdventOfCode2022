using MoreLinq;

class Day6
{
    public object A()
    {
        return File.ReadAllText("Day6.txt")
            .Index()
            .WindowLeft(4)
            .First(w => w.Select(q => q.Value).Distinct().Count() == 4)
            .Last().Key + 1;
    }
    
    public object B()
    {
        return File.ReadAllText("Day6.txt")
            .Index()
            .WindowLeft(14)
            .First(w => w.Select(q => q.Value).Distinct().Count() == 14)
            .Last().Key + 1;
    }
}