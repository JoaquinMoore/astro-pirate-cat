using UnityEngine;

public class StateAttack : NPCState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(NPC.transform.position, NPC.barco.transform.position) <= NPC.NPCData.ViewDistance)
        {
            Debug.Log("Atacando");
        }
        NPC.ApproachTo(NPC.barco.transform.position);
    }
}