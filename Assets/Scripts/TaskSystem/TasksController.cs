using System.Collections.Generic;

namespace TaskSystem
{
    public class TasksController<TContext>
    {
        public ITask<TContext> DefaultTask { get; set; }

        private readonly Queue<ITask<TContext>> _tasks = new();
        private ITask<TContext> _currentTask;
        private readonly TContext _context;

        public TasksController(TContext context)
        {
            _context = context;
        }

        public void AddTask(params ITask<TContext>[] newTasks)
        {
            foreach (var task in newTasks)
            {
                _tasks.Enqueue(task);
            }
        }

        public void CheckTask()
        {
            if (!_tasks.TryDequeue(out _currentTask))
            {
                _currentTask = DefaultTask;
            }

            _currentTask?.Update(_context);
        }
    }
}