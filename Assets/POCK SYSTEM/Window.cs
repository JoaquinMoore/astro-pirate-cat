using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace POCK
{
    public interface IOpenable
    {
        protected void Open(NPC actor);

        public class OpenAction : Interaction<NPC>
        {
            public OpenAction(IOpenable openable) : base("Open", _ => true, openable.Open)
            {
            }

            public OpenAction SetCondition(Func<NPC, bool> condition)
            {
                _condition = condition;
                return this;
            }
        }
    }

    public class Window : MonoBehaviour, IInteractable<NPC>, IOpenable
    {
        private readonly Dictionary<Enum, Interaction<NPC>> _interactions = new();
        Dictionary<Enum, Interaction<NPC>> IInteractable<NPC>.Interactions => _interactions;

        public Enum PossibleInteractions { get; private set; }
        private SpriteResolver _sr;

        [Flags]
        public enum Actions
        {
            Open = 1,
            Close = 1 << 1,
            Break = 1 << 2,
            Lock = 1 << 3,
            Unlock = 1 << 4,
            Repair = 1 << 5
        }

        private void Awake()
        {
            _sr = GetComponent<SpriteResolver>();
            _interactions.Add(Actions.Open, new IOpenable.OpenAction(this));
            // _interactions.Add(Actions.Open, new Interaction<NPC>("Abrir", _ => true, Open));
            _interactions.Add(Actions.Close, new Interaction<NPC>("Cerrar", _ => true, Close));
            _interactions.Add(Actions.Break, new Interaction<NPC>("Romper", _ => true, Break));
            _interactions.Add(Actions.Lock, new Interaction<NPC>("Bloquear", _ => true, Lock));
            _interactions.Add(Actions.Unlock, new Interaction<NPC>("Desbloquear", _ => true, Unlock));
            _interactions.Add(Actions.Repair, new Interaction<NPC>("Reparar", actor => actor.currentTool == NPC.Tool.Hammer, Repair));
            Close(null);
        }

        #region ACTIONS
        private void Close(NPC actor)
        {
            Debug.Log("Cerré");
            _sr.SetCategoryAndLabel("window", nameof(Actions.Close));
            PossibleInteractions = Actions.Open | Actions.Break | Actions.Lock;
        }

        private void Unlock(NPC actor)
        {
            Close(actor);
        }

        private void Lock(NPC actor)
        {
            _sr.SetCategoryAndLabel("window", nameof(Actions.Lock));
            PossibleInteractions = Actions.Break | Actions.Unlock;
        }

        private void Repair(NPC actor)
        {
            Debug.Log("Reparé");
            Close(actor);
        }

        private void Break(NPC actor)
        {
            Debug.Log("Rompí");
            _sr.SetCategoryAndLabel("window", nameof(Actions.Break));
            PossibleInteractions = Actions.Repair;
        }

        void IOpenable.Open(NPC actor)
        {
            Debug.Log("Abrí");
            _sr.SetCategoryAndLabel("window", nameof(Actions.Open));
            PossibleInteractions = Actions.Close | Actions.Break;
        }
        #endregion

    }
}