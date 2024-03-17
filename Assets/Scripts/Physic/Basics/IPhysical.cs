using UnityEngine;

namespace GameEngine.Physic
{
    public interface IPhysical
    {
        public Vector3 Velocity { get; }

        public void SetVelocity(Vector3 velocity);
        
        public void AddVelocity(Vector3 velocity);
    }
}