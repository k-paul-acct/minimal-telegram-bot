namespace MinimalTelegramBot.StateMachine.Abstractions;

public abstract class State : IEquatable<State>
{
    private readonly int _stateId;
    private readonly string _stateKey;
    public readonly IDictionary<string, object?> Data = new Dictionary<string, object?>();

    protected State(string stateKey, int stateId)
    {
        _stateKey = stateKey;
        _stateId = stateId;
    }

    public bool Equals(State? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _stateKey == other._stateKey && _stateId == other._stateId;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((State)obj);
    }

    public static bool operator ==(State? a, State? b)
    {
        return (a is null, b is null) switch
        {
            (true, true) => true,
            (false, false) => a!.Equals(b),
            _ => false,
        };
    }

    public static bool operator !=(State? a, State? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_stateKey, _stateId);
    }
}