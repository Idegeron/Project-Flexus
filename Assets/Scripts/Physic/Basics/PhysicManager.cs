using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace GameEngine.Physic
{
    public class PhysicManager : IFixedTickable
    {
        protected List<IPhysical> _physicals = new();

        public void Register(IPhysical physical)
        {
            if (!_physicals.Contains(physical))
            {
                _physicals.Add(physical);
            }
        }

        public void Unregister(IPhysical physical)
        {
            if (_physicals.Contains(physical))
            {
                _physicals.Remove(physical);
            }
        }
        
        public void FixedTick()
        {
            foreach (var physical in _physicals)
            {
                physical.SetVelocity(physical.Velocity + Physics.gravity * Time.fixedDeltaTime);
            }
        }
    }
}