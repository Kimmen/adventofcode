namespace Aoc.Day16;

internal partial class Valve
{
    public Valve(string name, int rate, string[] connectedValves)
    {
        Name = name;
        Rate = rate;
        ConnectedValves = connectedValves;
        Open = false;
    }

    public string Name { get; }
    public int Rate { get; }
    public string[] ConnectedValves { get; }
    public bool Open { get; set; }
}
