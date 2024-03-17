using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace GameEngine.Input
{
    public struct Pointer
    {
        public bool Contact;

        public Vector2 Position;

        public Vector2 Delta;

        public float? Pressure;
    }
    
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class PointerComposite : InputBindingComposite<Pointer>
    {
        [InputControl(layout = "Button")]
        public int contact;

        [InputControl(layout = "Integer")]
        public int inputId;

        [InputControl(layout = "Vector2")]
        public int position;

        [InputControl(layout = "Vector2")]
        public int delta;

        [InputControl(layout = "Axis")]
        public int pressure;

#if UNITY_EDITOR
        static PointerComposite()
        {
            Register();
        }
#endif

        public override Pointer ReadValue(ref InputBindingCompositeContext context)
        {
            var contact = context.ReadValueAsButton(this.contact);

            var position = context.ReadValue<Vector2, Vector2MagnitudeComparer>(this.position);

            var delta = context.ReadValue<Vector2, Vector2MagnitudeComparer>(this.delta);

            var pressure = context.ReadValue<float>(this.pressure);

            return new Pointer
            {
                Contact = contact,
                Position = position,
                Delta = delta,
                Pressure = pressure > 0 ? pressure : (float?)null
            };
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Register()
        {
            UnityEngine.InputSystem.InputSystem.RegisterBindingComposite<PointerComposite>();
        }
    }
}