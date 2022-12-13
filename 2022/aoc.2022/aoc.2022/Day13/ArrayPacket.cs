using System.Collections.Generic;

namespace Aoc.Day13;


public abstract class Packet { }

public sealed class ValuePacket : Packet
{
    public int Value { get; set; }
}
public partial class ArrayPacket : Packet
{
    private int _nextCounter = 0;
    public List<Packet> Packets { get; set; } = new List<Packet>();
    
    //Helper props
    public string? InputLine { get; set; }
    public ArrayPacket? Parent { get; set; }
    //----

    public void Add(Packet packet)
    {
        this.Packets.Add(packet);
    }

    internal Packet? Next()
    {
        if(_nextCounter < this.Packets.Count)
        {
            return this.Packets[this._nextCounter++];
        }

        return null;
    }

    internal void ResetNext() { this._nextCounter = 0; }
}
