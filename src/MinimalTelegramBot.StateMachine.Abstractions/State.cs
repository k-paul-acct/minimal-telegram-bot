namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Represents an abstract state in the state machine.
/// </summary>
public abstract class State : IEquatable<State>
{
    private readonly int _stateId;
    private readonly string _stateKey;

    /// <summary>
    ///     Initializes a new instance of the <see cref="State"/> class with the specified state ID.
    /// </summary>
    /// <param name="stateId">The unique identifier for the state.</param>
    protected State(int stateId)
    {
        _stateId = stateId;
        var type = GetType();
        _stateKey = type.FullName ?? type.Name;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="State"/> class with the specified state ID and key.
    /// </summary>
    /// <param name="stateId">The unique identifier for the state.</param>
    /// <param name="key">The unique key for the state group.</param>
    protected State(int stateId, string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        _stateId = stateId;
        _stateKey = key;
    }

    /// <summary>
    ///     Gets the data associated with the state.
    /// </summary>
    public IDictionary<string, object?> Data { get; } = new Dictionary<string, object?>();

    /// <summary>
    ///     Determines whether the specified state is equal to the current state.
    /// </summary>
    /// <param name="other">The state to compare with the current state.</param>
    /// <returns>true if the specified state is equal to the current state; otherwise, false.</returns>
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

    /// <summary>
    ///     Determines whether the specified object is equal to the current state.
    /// </summary>
    /// <param name="obj">The object to compare with the current state.</param>
    /// <returns>true if the specified object is equal to the current state; otherwise, false.</returns>
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

    /// <summary>
    ///     Determines whether two <see cref="State"/> instances are equal.
    /// </summary>
    /// <param name="a">The first state to compare.</param>
    /// <param name="b">The second state to compare.</param>
    /// <returns>true if the two states are equal; otherwise, false.</returns>
    public static bool operator ==(State? a, State? b)
    {
        if (a is not null)
        {
            return a.Equals(b);
        }

        return b is null;
    }

    /// <summary>
    ///     Determines whether two <see cref="State"/> instances are not equal.
    /// </summary>
    /// <param name="a">The first state to compare.</param>
    /// <param name="b">The second state to compare.</param>
    /// <returns>true if the two states are not equal; otherwise, false.</returns>
    public static bool operator !=(State? a, State? b)
    {
        return !(a == b);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(_stateId, _stateKey);
    }
}
