using System;

public class Condition : Node
{
    private readonly Func<bool> _condition;

    public Condition(string name, Func<bool> condition) : base(name)
    {
        currentChildIndex = 0;
        _condition = condition;
    }

    public override Status Process()
    {
        if (_condition.Invoke())
        {
            return children[0].Process();
        }
        else if (children.Count == 2)
        {
            return children[1].Process();
        }

        return Status.Failure;
    }
}
