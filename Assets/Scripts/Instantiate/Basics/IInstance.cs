using System;
using UnityEngine;

namespace GameEngine.Instantiate
{
    public interface IInstance
    {
        public GameObject GameObject { get; }
        
        public GameObject SourceObject { get; set; }
        
        public event Action<IInstance> Released;
    }
}