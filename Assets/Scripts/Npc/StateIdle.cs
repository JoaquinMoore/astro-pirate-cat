using Npc;
using UnityEngine;
using UnityServiceLocator;

public abstract class NPCState : StateMachineBehaviour
{
    protected static readonly int Attacking = Animator.StringToHash("Attacking");

    protected NPCController NPC { get; private set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPC = ServiceLocator.For(animator.gameObject).Get<NPCController>();
    }
}

public class StateIdle : NPCState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NPC.CheckTask();
        animator.SetBool(Attacking, NPC.barco);
    }
}