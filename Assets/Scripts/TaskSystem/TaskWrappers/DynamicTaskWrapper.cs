using System;
using System.Reflection;
using Npc;
using UnityEngine;

namespace TaskSystem.TaskWrappers
{
    public class DynamicTaskWrapper : ScriptableObject
    {
        public ITask<NPCController>[] types;

        public void Refresh()
        {
        }
    }
}