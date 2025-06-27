using System;
using DutySystem;

public class Character
{
    public void Awake()
    {
        (new Prueba() as IHinge<Character>).Interact(IHinge<Character>.Actions.Open, this);
    }
}

public interface IHinge<TData> : IInteractable<IHinge<TData>.Actions, TData>
{
    [Flags]
    public enum Actions
    {
        Open,
        Close
    }

    bool IInteractable<Actions, TData>.Check(Actions action, TData data)
    {
        return action switch
        {
            Actions.Open => CanOpen(data),
            Actions.Close => CanClose(data),
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }

    void IInteractable<Actions, TData>.Interact(Actions action, TData data)
    {
        switch (action)
        {
            case Actions.Open:
                Open(data);
                break;
            case Actions.Close:
                Close(data);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    protected void Open(TData data);
    protected void Close(TData data);
    protected bool CanOpen(TData data);
    protected bool CanClose(TData data);
}

public class Prueba : IHinge<Character>
{
    public Enum PossibleActions { get; private set; }

    public void Awake()
    {
        PossibleActions = IHinge<Character>.Actions.Open | IHinge<Character>.Actions.Close;
    }

    void IHinge<Character>.Open(Character actor)
    {
    }

    void IHinge<Character>.Close(Character actor)
    {
    }

    bool IHinge<Character>.CanOpen(Character actor)
    {
        return true;
    }

    bool IHinge<Character>.CanClose(Character actor)
    {
        return true;
    }
}