using VContainer;
using VContainer.Unity;

namespace GameEngine.Input
{
    public class InputManagerInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InputManager>().AsSelf();
        }
    }
}