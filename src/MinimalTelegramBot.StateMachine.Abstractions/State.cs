namespace MinimalTelegramBot.StateMachine.Abstractions;

public abstract class State : IEquatable<State>
{
    private readonly int _stateId;
    private readonly string _stateKey;
    public readonly IDictionary<string, object?> Data = new Dictionary<string, object?>();

    protected State(int stateId)
    {
        var type = GetType();
        _stateKey = type.FullName ?? type.Name;
        _stateId = stateId;
    }

    public bool Equals(State? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return _stateId == other._stateId && _stateKey == other._stateKey;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is State other && Equals(other);
    }

    public static bool operator ==(State? a, State? b)
    {
        if (a is not null)
        {
            return a.Equals(b);
        }

        return b is null;
    }

    public static bool operator !=(State? a, State? b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_stateId, _stateKey);
    }
}