using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace POCK
{
    public class NPC  :MonoBehaviour
    {
        public enum Tool
        {
            Pickaxe,
            Shovel,
            Hammer
        }

        public Tool currentTool;
        public float viewRadius;

        private void OnGUI()
        {
            IInteractable<NPC> window = null;
            if (Physics2D.OverlapCircleAll(transform.position, viewRadius).First(c => c.TryGetComponent(out window)))
            {
                foreach (var action in window.GetAvailableInteractions().ToList())
                {
                    GUI.color = action.CanExecute(this) ? Color.green : Color.red;
                    if (GUILayout.Button(action.Name))
                    {
                        action.Execute(this);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);
        }
    }

    public static class Extensions
    {
        public static IEnumerable<Interaction<TActor>> GetAvailableInteractions<TActor>(
            this IInteractable<TActor> interactable) => interactable.GetAvailableInteractions();

        public static void Interact<TActor>(this IInteractable<TActor> interactable, TActor actor, Enum action)
        {
            interactable.Interact(actor, action);
        }

        public static bool CanInteract<TActor>(this IInteractable<TActor> interactable, TActor actor, Enum action)
        {
            return interactable.CanInteract(actor, action);
        }
    }

    // public static class Prueba
    // {
    //     public static IInteractable<TContext> Interaction<TContext>(this IInteractable<TContext> interactable) => interactable;
    // }
}