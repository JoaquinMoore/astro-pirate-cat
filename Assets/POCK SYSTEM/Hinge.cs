using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace POCK
{
    public class Hinge : MonoBehaviour, IInteractable<NPC>
    {
        public UnityAction<NPC> OnOpen, OnClose;

        private Dictionary<Enum, Interaction<NPC>> _interactions = new();
        Dictionary<Enum, Interaction<NPC>> IInteractable<NPC>.Interactions => _interactions;

        public Enum PossibleInteractions { get; set; }

        [Flags]
        public enum Actions
        {
            Open = 1,
            Close = 1 << 1
        }

        private void Awake()
        {
            _interactions.Add(Actions.Open, new Interaction<NPC>("Abrir", _ => true, OnOpen.Invoke));
            _interactions.Add(Actions.Close, new Interaction<NPC>("Cerrar", _ => true, OnClose.Invoke));
        }
    }
}