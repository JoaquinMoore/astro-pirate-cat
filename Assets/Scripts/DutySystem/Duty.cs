using System;
using UnityEngine;

namespace DutySystem
{
    /// <summary>
    /// Base abstract class for all duty types
    /// </summary>
    [Serializable]
    public abstract class Duty
    {
        [SerializeField, HideInInspector]
        public string _name = "Task";

        /// <summary>
        /// Call the duty
        /// </summary>
        public abstract void Call();

        /// <summary>
        /// Creates a copy of this duty
        /// </summary>
        public abstract Duty Clone();
    }

    /// <summary>
    /// Generic duty that can interact with objects implementing IInteractable<TData>
    /// </summary>
    /// <typeparam name="TData">Type of data used in interaction</typeparam>
    [Serializable]
    public class Duty<TData> : Duty, ISerializationCallbackReceiver
    {
        [SerializeField]
        public TData data;

        [SerializeField, HideInInspector]
        protected UnityEngine.Object interactableReference;

        protected IInteractable<TData> interactable;
        protected IInteractable<TData> Interactable => interactable ??= (IInteractable<TData>)interactableReference;

        public override void Call()
        {
            Interactable.Interact(data);
        }

        public void OnBeforeSerialize()
        {
            if (interactableReference&& interactableReference is not IInteractable<TData>)
            {
                if (!interactableReference)
                {
                    Debug.LogWarning($"The referenced <{interactableReference}> does not implement IInteractable<TData> and cannot be used in a duty.");
                }
            }
        }

        public void OnAfterDeserialize()
        { }

        /// <summary>
        /// Sets the data for this duty
        /// </summary>
        public Duty<TData> SetData(TData data)
        {
            this.data = data;
            return this;
        }

        /// <summary>
        /// Sets the interactable object for this duty
        /// </summary>
        public Duty<TData> SetInteractable(IInteractable<TData> interactable)
        {
            this.interactable = interactable;
            interactableReference = interactable as UnityEngine.Object;
            return this;
        }

        public override Duty Clone()
        {
            return new Duty<TData>()
            {
                data = data,
                interactableReference = interactableReference,
                interactable = interactable
            };
        }
    }

    /// <summary>
    /// Generic duty that can interact with objects implementing IInteractable<TAction,TData>
    /// </summary>
    /// <typeparam name="TAction">Enum type defining possible actions</typeparam>
    /// <typeparam name="TData">Type of data used in interaction</typeparam>
    [Serializable]
    public class Duty<TAction, TData> : Duty<TData> where TAction : Enum
    {
        [SerializeField]
        private TAction action;

        public override void Call()
        {
            ((IInteractable<TAction, TData>)Interactable).Interact(action, data);
        }

        /// <summary>
        /// Sets the action for this duty
        /// </summary>
        public Duty<TAction, TData> SetAction(TAction action)
        {
            this.action = action;
            return this;
        }

        public override Duty Clone()
        {
            return new Duty<TAction, TData>()
            {
                data = data,
                interactable = interactable,
                interactableReference = interactableReference,
                action = action
            };
        }
    }
}