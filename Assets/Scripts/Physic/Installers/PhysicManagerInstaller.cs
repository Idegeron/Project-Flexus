using GameEngine.Physic;
using VContainer;
using VContainer.Unity;

namespace Physic.Installers
{
    public class PhysicManagerInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<PhysicManager>().AsSelf();
        }
    }
}