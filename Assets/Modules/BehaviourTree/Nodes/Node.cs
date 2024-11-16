using System.Collections.Generic;

public class Node
{
    public enum Status { Success, Failure, Running }

    public string Name => _name;

    public List<Node> children = new();

    protected int currentChildIndex;

    public Node(string name)
    {
        _name = name;
    }

    public void AddChild(Node child) => children.Add(child);

    public virtual Status Process()
    {
        var status = children[currentChildIndex].Process();
        return status;
    }

    public virtual void Reset()
    {
        currentChildIndex = 0;
        foreach (var child in children)
        {
            child.Reset();
        }
    }

    private readonly string _name;
}
