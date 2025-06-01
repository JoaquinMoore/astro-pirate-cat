using Npc;
using Npc.Tasks;

public class TaskInteract : BaseNPCTask
{
    public IInteractable target;

    public override void Execute(NPCFacade npc)
    {
        target.Interact(npc);
    }
}