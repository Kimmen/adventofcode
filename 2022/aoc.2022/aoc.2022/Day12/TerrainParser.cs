namespace Aoc.Day12;

public partial class Terrain
{
    public static (Terrain terrain, Elevation start, Elevation end) Parse(string input)
    {
        var terrain = new Terrain();
        var start = new Elevation(0, new Pos(0, 0));
        var end = new Elevation(0, new Pos(0, 0));
        var x = 0;
        var y = 0;
        
        foreach (var c in input)
        {
            switch (c)
            {
                case '\r':
                    continue;
                case '\n':
                    y++;
                    x = 0;
                    continue;
            }

            var current = new Elevation(ConvertToNumericElevation(c switch
            {
                'S' => 'a',
                'E' => 'z',
                _ => c
            }), new Pos(x, y));
            
            terrain.SetElevation(current);
            x++;

            switch (c)
            {
                case 'S':
                    start = current;
                    break;
                case 'E':
                    end = current;
                    break;
            }
        }

        return (terrain, start, end);
    }

    private static int ConvertToNumericElevation(char e)
    {
        return e - 'a';
    }
}