using System;

namespace DutySystem
{
    public static class DutyExtensions
    {
        public static Duty[] CloneDuty(this Duty[] duties)
        {
            var clones = new Duty[duties.Length];
            for (int i = 0; i < duties.Length; i++)
            {
                clones[i] = duties[i].Clone();
            }

            return clones;
        }

        public static Duty CreateDuty<TData>(this IInteractable<TData> interactable)
        {
            return new Duty<TData>().SetInteractable(interactable);
        }

        public static Duty CreateDuty<TData>(this IInteractable<TData> interactable, TData data)
        {
            return new Duty<TData>().SetData(data).SetInteractable(interactable);
        }

        public static Duty CreateDuty<TAction, TData>(this IInteractable<TAction, TData> interactable) where TAction : Enum
        {
            return new Duty<TAction, TData>().SetInteractable(interactable);
        }

        public static Duty CreateDuty<TAction, TData>(this IInteractable<TAction, TData> interactable, TData data) where TAction : Enum
        {
            return new Duty<TAction, TData>().SetData(data).SetInteractable(interactable);
        }

        public static Duty CreateDuty<TAction, TData>(this IInteractable<TAction, TData> interactable, TAction action) where TAction : Enum
        {
            return new Duty<TAction, TData>().SetAction(action).SetInteractable(interactable);
        }

        public static Duty CreateDuty<TAction, TData>(this IInteractable<TAction, TData> interactable, TAction action, TData data) where TAction : Enum
        {
            return new Duty<TAction, TData>().SetAction(action).SetData(data).SetInteractable(interactable);
        }
    }
}