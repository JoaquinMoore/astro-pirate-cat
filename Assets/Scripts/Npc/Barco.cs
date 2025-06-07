using System;
using TasksSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Npc
{
    public interface IFocusable : IInteractable<NPCController>
    {
        void IInteractable<NPCController>.Interact(NPCController data)
        {
            Focus(data);
        }

        bool IInteractable<NPCController>.CanInteract(NPCController data)
        {
            throw new NotImplementedException();
        }

        protected void Focus(NPCController npc);
    }

    public class Barco : MonoBehaviour, IInteractable<Barco.Actions, Barco.Data>, IFocusable
    {
        public Enum PossibleActions { get; }

        [Flags]
        public enum Actions
        {
            Pintar = 1,
            Nombrar = 2,
        }

        [Serializable]
        public struct Data
        {
            public Color Pintura;
            public float Cantidad;
            public string Nombre;
        }

        public void Interact(Actions action, Data data)
        {
            for (var flag = (Actions)1; flag <= action; flag = (Actions)((int)flag << 1))
            {
                if ((action & flag) == 0)
                    continue;

                switch (flag)
                {
                    case Actions.Pintar:
                        Pintar(data.Pintura);
                        break;
                    case Actions.Nombrar:
                        Renombrar(data.Nombre);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // switch (action)
            // {
            //     case Actions.Pintar:
            //         Pintar(data.Pintura);
            //         break;
            //     case Actions.Nombrar:
            //         Renombrar(data.Nombre);
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }

        private void Pintar(Color color)
        {
            GetComponent<SpriteRenderer>().color = color;
        }

        private void Renombrar(string nombre)
        {
            gameObject.name = nombre;
        }

        bool IInteractable<Actions, Data>.Check(Actions action, Data data)
        {
            return true;
        }

        [ContextMenu("Interact Random")]
        public void Interact()
        {
            Interact(Actions.Pintar | Actions.Nombrar, new Data(){Pintura = Random.ColorHSV(), Nombre = "Pepito"});
        }

        void IFocusable.Focus(NPCController npc)
        {
            npc.barco = this;
        }

        public Task[] CreateBaseTask()
        {
            return new[]
            {
                this.CreateTask(),
                ((IFocusable)this).CreateTask()
            };
        }
    }
}