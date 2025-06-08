using System;
using System.Threading.Tasks;
using _EXTENSIONS;
using Npc;
using UnityEngine;

public interface ITask<TContext>
{
    bool IsComplete { get; }

    internal bool HasStarted { get; set; }

    bool Update(TContext context)
    {
        if (!HasStarted)
        {
            Start(context);
            HasStarted = true;
        }
        else if (!IsComplete)
        {
            Execute(context);
        }

        return IsComplete;
    }

    protected void Execute(TContext context);
    protected void Start(TContext context);
    ITask<TContext> Clone();
}

public interface ITask<TContext, TData> : ITask<TContext>
{
    internal TData Data { get; set; }

    ITask<TContext, TData> SetData(TData data)
    {
        Data = data;
        return this;
    }
}

[Serializable]
public class AttackShip : ITask<NPCController>
{
    public string Name = "Attack ship";
    private bool _hasStarted;
    public bool IsComplete { get; private set; }

    bool ITask<NPCController>.HasStarted
    {
        get => _hasStarted;
        set => _hasStarted = value;
    }

    void ITask<NPCController>.Execute(NPCController context)
    {
        if (context.transform.position.AreApproximately(Barco.Instance.transform.position))
        {
            IsComplete = true;
            context.barco = null;
            Debug.Log("Logré completar la tarea");
        }
    }

    void ITask<NPCController>.Start(NPCController context)
    {
        context.barco = Barco.Instance;
    }

    public ITask<NPCController> Clone()
    {
        return new AttackShip();
    }
}