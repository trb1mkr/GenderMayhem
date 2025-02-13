using UnityEngine;

public class RandomExtra<T>
{
    public static T Choose(T[] variants)
    {
        var result = Random.Range(0, variants.Length);
        return variants[result];
    }
}

public class RandomExtra
{
    public static int Sign()
    {
        int factor = Random.Range(0, 2);
        if (factor == 0) factor = -1;
        return factor;
    }
}