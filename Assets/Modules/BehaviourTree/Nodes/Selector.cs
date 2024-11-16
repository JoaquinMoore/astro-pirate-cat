public class Selector : Node
{
    public Selector(string name) : base(name) {}

    public override Status Process()
    {
        if (currentChildIndex < children.Count)
        {
            switch(base.Process())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Success:
                    return Status.Success;
                default:
                    currentChildIndex++;
                    return Status.Running;
            }
        }

        Reset();
        return Status.Failure;
    }
}
