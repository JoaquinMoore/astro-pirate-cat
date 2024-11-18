using UnityServiceLocator;

public class MainCharacterInstaller : InstallerBase
{
    protected override void Awake()
    {
        ServiceLocator.For(this).Register<IBrain<MainCharacterController>>(new MainCharacterPlayerBrain());
    }
}
