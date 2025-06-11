namespace TaskSystem
{
    public interface ITask<TContext>
    {
        bool IsComplete { get; }

        protected bool HasStarted { get; set; }

        bool Update(TContext context)
        {
            if (!HasStarted)
            {
                HasStarted = true;
                Start(context);
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
}