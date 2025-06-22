using System;
using System.Collections.Generic;
using UnityEngine;

namespace POCK
{
    public class Door : MonoBehaviour, IInteractable<NPC>
    {
        private Dictionary<Enum, Interaction<NPC>> _interactions = new();

        [Flags]
        public enum Actions
        {
            Open = 1,
            Close = 1 << 1
        }

        private void Awake()
        {
            _interactions.Add(Actions.Open, new Interaction<NPC>("Abrir", _ => true, Open));
            _interactions.Add(Actions.Close, new Interaction<NPC>("Cerrar", _ => true, Close));
            Close(null);
        }

        private void Open(NPC actor)
        {
            Debug.Log("Abrir");
            PossibleInteractions = Hinge.Actions.Close;
        }

        private void Close(NPC actor)
        {
            Debug.Log("Cerrar");
            PossibleInteractions = Hinge.Actions.Open;
        }

        Dictionary<Enum, Interaction<NPC>> IInteractable<NPC>.Interactions => _interactions;

        public Enum PossibleInteractions { get; private set; }
    }
}