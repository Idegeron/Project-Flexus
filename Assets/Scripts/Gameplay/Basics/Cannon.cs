using System.Collections.Generic;
using GameEngine.Common;
using GameEngine.Instantiate;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace GameEngine.Gameplay
{
    public class Cannon : SerializedMonoBehaviour
    {
        [SerializeField, TabGroup("Components")]
        protected Transform _shootPointTransform;

        [SerializeField, TabGroup("Components")]
        protected Transform _projectileContainer;
        
        [SerializeField, TabGroup("Components")]
        protected GameObject _projectilePrefab;

        [SerializeField, TabGroup("Components")]
        protected Animator _animator;
        
        [SerializeField, TabGroup("Parameters")]
        protected float _power;

        [SerializeField, TabGroup("Parameters")]
        protected float _minProjectileSize;

        [SerializeField, TabGroup("Parameters")]
        protected float _maxProjectileSize;
        
        [SerializeField, TabGroup("Parameters")]
        protected float _minProjectileStrength;

        [SerializeField, TabGroup("Parameters")]
        protected float _maxProjectileStrength;

        [SerializeField, TabGroup("Parameters")]
        protected string _shootKey;

        [SerializeField, TabGroup("Parameters")]
        protected string _shakeKey;
        
        protected InstantiateManager _instantiateManager;

        protected List<Mesh> _projectileMeshes = new();

        public float Power => _power;

        [Inject]
        protected void Construct(InstantiateManager instantiateManager)
        {
            _instantiateManager = instantiateManager;
        }

        protected void ProjectileReleasedHandler(IInstance instance)
        {
            instance.Released -= ProjectileReleasedHandler;
            
            _animator.SetTrigger(_shakeKey);
        }
        
        protected void Awake()
        {
            for (var i = 0; i < Random.Range(3, 7); i++)
            {
                _projectileMeshes.Add(MeshGenerator.CreateCube(Random.Range(_minProjectileSize, _maxProjectileSize), 
                    Random.Range(_minProjectileStrength, _maxProjectileStrength)));
            }
        }

        public virtual void SetPower(float value)
        {
            _power = value;
        }

        public virtual void Shoot()
        {
            var projectile = _instantiateManager.Instantiate<Projectile>(_projectilePrefab, 
                _shootPointTransform.position, _shootPointTransform.rotation, _projectileContainer);
            
            projectile.SetMesh(_projectileMeshes[Random.Range(0, _projectileMeshes.Count)]);
            
            projectile.Shoot(_power);
            
            projectile.Released += ProjectileReleasedHandler;
            
            _animator.SetTrigger(_shootKey);
        }
    }
}