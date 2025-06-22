using System;
namespace Otro
{
    public interface IInteractable<TAction> where TAction : Enum
    {
        void Interact(TAction action);
    }

    public interface IInteractable<TAction, TActor> where TAction : Enum
    {
        Enum possibleActions { get; }
        TActor Actor { get; }

        void Interact(TAction action, TActor actor);
    }

    public interface IHinge<TActor> : IInteractable<IHinge<TActor>.Actions, TActor>
    {
        enum Actions
        {
            Open,
            Close
        }

        void IInteractable<Actions, TActor>.Interact(Actions action, TActor actor)
        {
            switch (action)
            {
                case Actions.Open:
                    Open();
                    break;
                case Actions.Close:
                    Close();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        protected void Open();
        protected void Close();
    }

    public class Window : IInteractable<Window.Interactions, Character>, IHinge<Character>
    {
        public enum Interactions
        {
            Lock,
            Force,
            Break
        }

        public Enum possibleActions { get; }
        public Character Actor { get; }
        public void Interact(Interactions action, Character actor)
        {
            throw new NotImplementedException();
        }

        public void Interact(Enum action, Character actor)
        {
            switch (action)
            {
                case Interactions.Lock:
                    break;
                case Interactions.Force:
                    break;
                case Interactions.Break:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        void IHinge<Character>.Open()
        {
        }

        void IHinge<Character>.Close()
        {
            throw new NotImplementedException();
        }
    }

    public class Character
    {
        private Window _window;

        public void Update()
        {
            // _window.Interact(IHinge<Character>.Actions.Open, this);
        }
    }
}