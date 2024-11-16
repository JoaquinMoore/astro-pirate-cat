public class Sequence : Node
{
    public Sequence(string name) : base(name) {}

    public override Status Process()
    {
        if (currentChildIndex < children.Count)
        {
            switch(base.Process())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                    Reset(); 
                    return Status.Failure;
                default:
                    currentChildIndex++;
                    return currentChildIndex == children.Count ? Status.Success : Status.Running;
            }
        }

        Reset();
        return Status.Success;
    }
}
