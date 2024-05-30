using System;
using System.Security.Cryptography;

public class StrongRandom
{
    public static Random RNG { get; private set; }

    static StrongRandom()
    {
        var randomNumberGenerator = RandomNumberGenerator.Create();
        RNG = new(RandomNumberGenerator.GetInt32(int.MaxValue));
        randomNumberGenerator.Dispose();
    }
}
