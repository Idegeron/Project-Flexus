using VContainer;
using VContainer.Unity;

namespace GameEngine.Instantiate
{
    public class InstantiateManagerInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InstantiateManager>().AsSelf();
        }
    }
}