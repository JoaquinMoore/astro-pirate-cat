public class BehaviourTree : Node
{
    public BehaviourTree(string name) : base(name) {}

    public override Status Process()
    {
        while (currentChildIndex < children.Count)
        {
            var status = base.Process();

            if (status != Status.Success)
            {
                return status;
            }
            currentChildIndex++;
        }

        Reset();
        return Status.Success;
    }
}
