using System;
using System.Collections.Generic;
using System.Linq;

namespace POCK
{
    public interface IInteractable<TActor>
    {
        protected Dictionary<Enum, Interaction<TActor>> Interactions { get; }
        Enum PossibleInteractions { get; }

        IEnumerable<Interaction<TActor>> GetAvailableInteractions()
        {
            return from
                interaction in
                Interactions where
                (Convert.ToInt32(PossibleInteractions) & Convert.ToInt32(interaction.Key)) != 0 select
                interaction.Value;
        }

        void Interact(TActor actor, Enum interaction)
        {
            var action = Interactions[interaction];
            if (action.CanExecute(actor))
            {
                action.Execute(actor);
            }
        }

        bool CanInteract(TActor actor, Enum interaction)
        {
            return (Convert.ToInt32(PossibleInteractions) & Convert.ToInt32(interaction)) != 0 && Interactions[interaction].CanExecute(actor);
        }
    }

    public class Interaction<TActor>
    {
        public string Name { get; private set; }
        protected Func<TActor, bool> _condition;
        protected Action<TActor> _action;

        public Interaction(string name, Func<TActor, bool> condition, Action<TActor> action)
        {
            Name = name;
            _condition = condition;
            _action = action;
        }

        public bool CanExecute(TActor actor) => _condition(actor);

        public void Execute(TActor actor)
        {
            if (CanExecute(actor))
            {
                _action(actor);
            }
        }
    }
}