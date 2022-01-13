using NBitcoin;

namespace EagleSabi.Infrastructure.Common.Modules;

internal class UnguessableGuidGenerator : IUnguessableGuidGenerator
{
    public UnguessableGuidGenerator(IRandom secureRandom)
    {
        SecureRandom = secureRandom;
    }

    public IRandom SecureRandom { get; }

    public Guid NewGuid()
    {
        // Generate a new GUID with the secure random source, to be sure
        // that it is not guessable (Guid.NewGuid() documentation does
        // not say anything about GUID version or randomness source,
        // only that the probability of duplicates is very low).
        var buffer = new byte[16];
        SecureRandom.GetBytes(buffer);
        return new Guid(buffer); // technically this is not a valid guid but it shouldn't matter
    }
}