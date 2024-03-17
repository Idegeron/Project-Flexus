using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.Gameplay
{
    public class TrajectoryController : SerializedMonoBehaviour
    {
        [SerializeField, TabGroup("Components")]
        protected Cannon _cannon;

        [SerializeField, TabGroup("Components")]
        protected Transform _directionTransform;
        
        [SerializeField, TabGroup("Components")]
        protected LineRenderer _lineRenderer;

        [SerializeField, TabGroup("Parameters")]
        protected int _positionCount;
        
        protected virtual void Start()
        {
            _lineRenderer.positionCount = _positionCount;
        }
        
        protected virtual void LateUpdate()
        {
            _lineRenderer.SetPositions(GetPositions(_positionCount));
        }

        protected virtual void OnEnable()
        {
            _lineRenderer.gameObject.SetActive(true);
        }

        protected virtual void OnDisable()
        {
            _lineRenderer.gameObject.SetActive(false);
        }

        protected virtual Vector3[] GetPositions(int numberOfPoints)
        {
            var points = new Vector3[numberOfPoints];
            
            var timeStep = 0.1f;

            var initialPosition = _directionTransform.transform.position;

            var initialVelocity = _directionTransform.forward * _cannon.Power;

            for (var i = 0; i < numberOfPoints; i++)
            {
                var time = i * timeStep;

                var x = initialPosition.x + initialVelocity.x * time;
                
                var y = initialPosition.y + initialVelocity.y * time - 0.5f * Physics.gravity.magnitude * time * time;
                
                var z = initialPosition.z + initialVelocity.z * time;

                points[i] = new Vector3(x, y, z);
            }

            return points;
        }
    }
}