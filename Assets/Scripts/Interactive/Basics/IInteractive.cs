using System;

namespace GameEngine.Interactive
{
    public interface IInteractive
    {
        public event Action<IInteractiveParameters> Interacted;
    }
}