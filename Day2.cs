class Day2
{
    public int? A()
    {
        int totalPoints = 0;
        var transalate = new Dictionary<char, char>
        {
            { 'X', 'A' },
            { 'Y', 'B' },
            { 'Z', 'C' },
        };
        var beats = new Dictionary<char, char>
        {
            { 'A', 'C' },
            { 'B', 'A' },
            { 'C', 'B' },
        };

        foreach (var line in File.ReadAllLines("Day2.txt"))
        {
            var opponent = line[0];
            var my = transalate[line[2]];
            var score = 0;
            if (beats[my] == opponent)
                score = 6;
            else if (my == opponent)
                score = 3;
            score += my - 'A' + 1;
            totalPoints += score;
        }

        return totalPoints;
    }

    public int? B()
    {
        // Rock - paper - sissors
        var loss = new[] { 2, 0, 1 };
        var win = new[] { 1, 2, 0 };
        int totalScore = 0;
        foreach (var line in File.ReadAllLines("Day2.txt"))
        {
            var opponent = line[0];
            var ending = line[2];
            var score = (ending - 'X') * 3;
            if (score == 0)
                score += loss[opponent - 'A'] + 1;
            else if (score == 3)
                score += opponent - 'A' + 1;
            else
                score += win[opponent - 'A'] + 1;
            totalScore += score;
        }

        return totalScore;
    }
}