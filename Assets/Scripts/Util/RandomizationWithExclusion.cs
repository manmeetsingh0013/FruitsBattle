using System.Collections.Generic;
using System;

public static class RandomizationWithExclusion
{
    private static HashSet<int> excludedNumbers = new HashSet<int>();

    public static int GetRandomWithExclusion(Random random, int max)
    {
        int number;
        while(true)
        {
            number = random.Next(0, max + 1);
            if(excludedNumbers.Contains(number))
            {
                continue;
            }
            excludedNumbers.Add(number);
            break;
        }
        return number;
    }

    public static void ClearExcludedList()
    {
        excludedNumbers.Clear();
    }
}
