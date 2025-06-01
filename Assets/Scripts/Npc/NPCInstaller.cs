using UnityEngine;
using Zenject;

namespace Npc
{
    [CreateAssetMenu(fileName = "NPCInstaller", menuName = "Installers/NPCInstaller")]
    public class NPCInstaller : ScriptableObjectInstaller<NPCInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}