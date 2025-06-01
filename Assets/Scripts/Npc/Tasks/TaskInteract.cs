using System.Collections;
using _EXTENSIONS;
using Npc;
using Npc.Tasks;
using UnityEngine;

public class TaskInteract : BaseNPCTask
{
    private const float INTERACTION_THRESHOLD = 1f;

    [SerializeField] private int numero;

    private IInteractable _target;

    public override void Execute(NPCFacade npc)
    {
        Debug.Log(npc);
        npc.StartCoroutine(Go(npc));
    }

    private IEnumerator Go(NPCFacade npc)
    {
        while (!npc.transform.position.AreApproximately(_target.GetInteractionPosition(npc.transform.position), INTERACTION_THRESHOLD))
        {
            npc.GoTo(_target.GetInteractionPosition(npc.transform.position));
            yield return null;
        }
        _target.Interact(npc);
    }

    public override string Log()
    {
        return $"El npc tiene que interactuar con <{_target}>";
    }
}