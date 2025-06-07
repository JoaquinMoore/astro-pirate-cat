using TasksSystem;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Npc
{
    public class NPCSpawner : MonoBehaviour
    {
        [SerializeReference]
        private NPCController _npcRef;
        [SerializeField]
        private Object _interactable;
        [SerializeReference]
        private Task[] _tasks;

        private Task<NPCController> _focusTask;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += () =>
            {
                if (!_interactable)
                {
                    _tasks = null;
                    return;
                }

                if (_interactable is not IInteractable interactable)
                {
                    interactable = _interactable.GetComponentInChildren<IInteractable>();
                    _interactable = interactable as Object;

                    if (!_interactable)
                    {
                        _tasks = null;
                        Debug.LogWarning("El objeto no es interactuable");
                        return;
                    }
                }
                else
                {
                    var newTask = interactable.CreateBaseTask();
                    if (newTask.GetType() == _tasks.GetType())
                    {
                        return;
                    }
                }

                _tasks = interactable.CreateBaseTask();
            };
        }
        #endif

        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var npc = Instantiate(_npcRef);

            foreach (var task in _tasks)
            {
                var interactable = FindFirstObjectByType(_interactable.GetType()) as IInteractable;

                if (task.IsConvertibleTo<IInteractable<NPCController>>(true))
                {
                    ((Task<NPCController>)task).data = npc;
                }
            }
            _focusTask = FindFirstObjectByType<Barco>().CreateTask(npc) as Task<NPCController>;
            npc.DefaultTask = _focusTask;
        }
    }
}