public class Leaf : Node
{
    private readonly IStrategy _strategy;

    public Leaf(string name, IStrategy strategy): base(name)
    {
        _strategy = strategy;
    }

    public override Status Process() => _strategy.Process();

    public override void Reset() => _strategy.Reset();
}
