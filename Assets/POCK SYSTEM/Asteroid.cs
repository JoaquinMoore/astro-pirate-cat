using System;
using System.Collections.Generic;
using UnityEngine;

namespace POCK
{
    // public class Asteroid : MonoBehaviour, IInteractable<NPC>
    // {
    //     public Mineral[] minerals;
    //     private Dictionary<Type, Interaction<NPC>> _interactions = new();
    //
    //     private void Awake()
    //     {
    //         _interactions.Add(typeof(Minar), new Minar(this));
    //     }
    //
    //     public Mineral[] Mine()
    //     {
    //         return minerals;
    //     }
    //
    //     [Serializable]
    //     public struct Mineral
    //     {
    //         public enum MineralType
    //         {
    //             Diamond,
    //             Gold,
    //             Iron
    //         }
    //
    //         public MineralType type;
    //         public int amount;
    //     }
    //
    //     Dictionary<Type, Interaction<NPC>> IInteractable<NPC>.Interactions => _interactions;
    // }
}