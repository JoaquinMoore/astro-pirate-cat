using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "NPCInstaller", menuName = "Installers/NPCInstaller")]
public class NPCInstaller : ScriptableObjectInstaller<NPCInstaller>
{
    public override void InstallBindings()
    {
    }
}