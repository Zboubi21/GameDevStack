using UnityEngine;
using GameDevStack;

namespace GameDevStack
{
    public static class NumberExtensions
    {
        public static float DistanceWith(this float a, float b)
        {
            return Mathf.Abs(a - b);
        }

        private static int DistanceWith(this int a, int b)
        {
            return Mathf.Abs(a - b);
        }
    }
}

namespace GameDevStack
{
    public static class Utilities
    {
        public static float Distance(float a, float b)
        {
            return Mathf.Abs(a - b);
        }
    }
}

public class Test
{
    private void Start()
    {
        float f = 1;
        f.DistanceWith(-2);
        Utilities.Distance(1, 2);
    }
}