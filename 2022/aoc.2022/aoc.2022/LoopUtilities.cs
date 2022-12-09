﻿using System;

namespace Aoc;

public class LoopUtilities
{
    public static void Repeat(int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            action();
        }
    }
}