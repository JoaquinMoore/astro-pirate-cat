using Npc.Tasks.Interfaces;
using UnityEngine;

namespace Npc.Tasks
{
    public class TaskMove : ITask
    {
        private readonly Vector2 _position;

        public TaskMove(Vector2 position)
        {
            _position = position;
        }

        public void Execute(NPCController npc)
        {
            npc.GoTo(_position);
        }
    }
}