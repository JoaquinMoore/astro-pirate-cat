using System.Collections;
using AstroCat.NPC;
using UnityEngine;

public interface INPCTask
{
    void Execute(NPCController controller);
}

public class TaskDie : INPCTask
{
    public void Execute(NPCController controller)
    {
        Object.Destroy(controller.gameObject);
    }
}

public class TaskWait : INPCTask
{
    public void Execute(NPCController controller)
    {
        controller.StartCoroutine(Wait());
    }

    private static IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
}