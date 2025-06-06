using System;

namespace TasksSystem
{
    public static class TaskExtensions
    {
        public static Task[] CloneTask(this Task[] tasks)
        {
            var clones = new Task[tasks.Length];
            for (int i = 0; i < tasks.Length; i++)
            {
                clones[i] = tasks[i].Clone();
            }

            return clones;
        }

        public static Task CreateTask<TData>(this IInteractable<TData> interactable)
        {
            return new Task<TData>().SetInteractable(interactable);
        }

        public static Task CreateTask<TData>(this IInteractable<TData> interactable, TData data)
        {
            return new Task<TData>().SetData(data).SetInteractable(interactable);
        }

        public static Task CreateTask<TAction, TData>(this IInteractable<TAction, TData> interactable) where TAction : Enum
        {
            return new Task<TAction, TData>().SetInteractable(interactable);
        }

        public static Task CreateTask<TAction, TData>(this IInteractable<TAction, TData> interactable, TData data) where TAction : Enum
        {
            return new Task<TAction, TData>().SetData(data).SetInteractable(interactable);
        }

        public static Task CreateTask<TAction, TData>(this IInteractable<TAction, TData> interactable, TAction action) where TAction : Enum
        {
            return new Task<TAction, TData>().SetAction(action).SetInteractable(interactable);
        }

        public static Task CreateTask<TAction, TData>(this IInteractable<TAction, TData> interactable, TAction action, TData data) where TAction : Enum
        {
            return new Task<TAction, TData>().SetAction(action).SetData(data).SetInteractable(interactable);
        }
    }
}