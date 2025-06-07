using System;
using Unity.VisualScripting;
using UnityEngine;

namespace TasksSystem
{
    /// <summary>
    /// Base abstract class for all task types
    /// </summary>
    [Serializable]
    public abstract class Task
    {
        [SerializeField, HideInInspector]
        public string _name = "Task"; // string.Join(' ', GetType().GetGenericArguments().Select(t => t));

        /// <summary>
        /// Executes the task
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Creates a copy of this task
        /// </summary>
        public abstract Task Clone();
    }

    /// <summary>
    /// Generic task that can interact with objects implementing IInteractable<TData>
    /// </summary>
    /// <typeparam name="TData">Type of data used in interaction</typeparam>
    [Serializable]
    public class Task<TData> : Task, ISerializationCallbackReceiver
    {
        [SerializeField]
        public TData data;
        [SerializeField, HideInInspector]
        protected UnityEngine.Object interactableReference;
        protected IInteractable<TData> interactable;

        protected IInteractable<TData> Interactable => interactable ??= (IInteractable<TData>)interactableReference;

        public override void Execute()
        {
            Interactable.Interact(data);
        }

        public void OnBeforeSerialize()
        {
            if (interactableReference&& interactableReference is not IInteractable<TData>)
            {
                interactableReference = interactableReference.GetComponentInChildren<IInteractable<TData>>() as UnityEngine.Object;

                if (!interactableReference)
                {
                    Debug.LogWarning($"The referenced <{interactableReference}> does not implement IInteractable<TData> and cannot be used in a task.");
                }
            }
        }

        public void OnAfterDeserialize()
        { }

        /// <summary>
        /// Sets the data for this task
        /// </summary>
        public Task<TData> SetData(TData data)
        {
            this.data = data;
            return this;
        }

        /// <summary>
        /// Sets the interactable object for this task
        /// </summary>
        public Task<TData> SetInteractable(IInteractable<TData> interactable)
        {
            this.interactable = interactable;
            interactableReference = interactable as UnityEngine.Object;
            return this;
        }

        public override Task Clone()
        {
            return new Task<TData>()
            {
                data = data,
                interactableReference = interactableReference,
                interactable = interactable
            };
        }
    }

    /// <summary>
    /// Generic task that can interact with objects implementing IInteractable<TAction,TData>
    /// </summary>
    /// <typeparam name="TAction">Enum type defining possible actions</typeparam>
    /// <typeparam name="TData">Type of data used in interaction</typeparam>
    [Serializable]
    public class Task<TAction, TData> : Task<TData> where TAction : Enum
    {
        [SerializeField]
        private TAction action;

        public override void Execute()
        {
            ((IInteractable<TAction, TData>)Interactable).Interact(action, data);
        }

        /// <summary>
        /// Sets the action for this task
        /// </summary>
        public Task<TAction, TData> SetAction(TAction action)
        {
            this.action = action;
            return this;
        }

        public override Task Clone()
        {
            return new Task<TAction, TData>()
            {
                data = data,
                interactable = interactable,
                interactableReference = interactableReference,
                action = action
            };
        }
    }
}