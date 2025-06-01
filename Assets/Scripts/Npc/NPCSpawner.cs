using Npc;
using Npc.Tasks;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public NPCFacade npcRef;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        var npc = Instantiate(npcRef);
        // npc.AddTask();
    }

    [ContextMenu("Create Interact Task")]
    public void CreateInteractTask()
    {
    }
}