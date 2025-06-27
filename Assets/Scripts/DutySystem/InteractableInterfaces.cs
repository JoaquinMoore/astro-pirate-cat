using System;

namespace DutySystem
{
    /// <summary>
    /// Base interface for all interactable objects
    /// </summary>
    public interface IInteractable
    {
        Duty[] CreateDuty();
    }

    /// <summary>
    /// Interface for interactable objects that work with a specific data type
    /// </summary>
    /// <typeparam name="TData">Type of data used in interaction</typeparam>
    public interface IInteractable<TData> : IInteractable
    {
        bool CanInteract(TData data);
        void Interact(TData data);

        Duty[] IInteractable.CreateDuty() => CreateDuty();
    }

    /// <summary>
    /// Interface for interactable objects that work with specific actions and data
    /// </summary>
    /// <typeparam name="TAction">Enum type defining possible actions</typeparam>
    /// <typeparam name="TData">Type of data used in interaction</typeparam>
    public interface IInteractable<TAction, TData> : IInteractable<TData> where TAction : Enum
    {
        Enum PossibleActions { get; }

        void IInteractable<TData>.Interact(TData data)
        {
            Interact(default, data);
        }

        bool IInteractable<TData>.CanInteract(TData data)
        {
            return CanInteract(default, data);
        }

        bool CanInteract(TAction action, TData data)
        {
            return PossibleActions.HasFlag(action) && Check(action, data);
        }

        protected bool Check(TAction action, TData data);

        void Interact(TAction action, TData data);

        Duty[] IInteractable.CreateDuty() => CreateDuty();
    }
}