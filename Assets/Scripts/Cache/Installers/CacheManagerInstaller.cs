using VContainer;
using VContainer.Unity;

namespace GameEngine.Cache
{
    public class CacheManagerInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CacheManager>().AsSelf();
        }
    }
}