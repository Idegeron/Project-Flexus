using System;
using UnityEngine;

namespace GameEngine.Input
{
    [Serializable]
    public class Touch
    {
        [SerializeField]
        private bool _contact;

        [SerializeField] 
        private Vector2 _currentPosition;

        [SerializeField] 
        private Vector2 _initPosition;

        [SerializeField] 
        private Vector2 _finalPosition;

        [SerializeField] 
        private Vector2 _delta;

        [SerializeField] 
        private Vector2 _absoluteDelta;

        [SerializeField] 
        private float? _pressure;

        public bool Contact => _contact;

        public Vector2 CurrentPosition => _currentPosition;

        public Vector2 InitPosition => _initPosition;

        public Vector2 FinalPosition => _finalPosition;

        public Vector2 Delta => _delta;

        public Vector2 AbsoluteDelta => _absoluteDelta;

        public float? Pressure => _pressure;

        public event Action<Touch> Press;

        public event Action<Touch> Hold;

        public event Action<Touch> Release;

        public void Update(Pointer pointer)
        {
            if (!Contact && pointer.Contact)
            {
                _contact = pointer.Contact;

                _currentPosition = pointer.Position;

                _initPosition = pointer.Position;

                _finalPosition = pointer.Position;

                _delta = pointer.Delta;

                _absoluteDelta = _currentPosition - _initPosition;

                _pressure = pointer.Pressure;

                Press?.Invoke(this);
            }
            else if (Contact && pointer.Contact)
            {
                _currentPosition = pointer.Position;

                _delta = pointer.Delta;
                
                _absoluteDelta = _currentPosition - _initPosition;

                _pressure = pointer.Pressure;

                Hold?.Invoke(this);
            }
            else
            {
                _contact = pointer.Contact;

                _currentPosition = pointer.Position;

                _finalPosition = pointer.Position;

                _delta = pointer.Delta;
                
                _absoluteDelta = _currentPosition - _initPosition;

                _pressure = pointer.Pressure;

                Release?.Invoke(this);
            }
        }
    }
}