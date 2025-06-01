using Npc;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public NPCFacade npcRef;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        var npc = Instantiate(npcRef);
    }
}