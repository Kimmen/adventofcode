using System.Collections.Generic;

namespace Aoc.Day13;

public partial class ArrayPacket
{
    public static ArrayPacket Parse(string input)
    {
        var i = 1;
        var currentArray = new ArrayPacket() { InputLine = input };
        while (i < input.Length)
        {
            var current = input[i];

            if (current == '[')
            {
                var newArray = new ArrayPacket
                {
                    Parent = currentArray
                };
                currentArray.Add(newArray);
                currentArray = newArray;

            }
            else if (current == ']')
            {
                currentArray = currentArray.Parent ?? currentArray;
            }
            else if (char.IsDigit(current))
            {
                var valuePacket = ParseValue(input, ref i);
                currentArray.Add(valuePacket);
            }

            i++;
        }

        return currentArray;
    }

    private static ValuePacket ParseValue(string input, ref int i)
    {
        var current = input[i];
        var valueInput = new List<char>();
        while (char.IsDigit(current))
        {
            valueInput.Add(current);
            current = input[++i];
        }
        //unparse last char, let the main loop take care of it.
        i--;

        return new ValuePacket
        {
            Value = int.Parse(valueInput.ToArray()),
        };
    }
}