///<summary>
/// Author: Halen
///
/// Vector2 processor that scales with the width or height of the screen.
///
///</summary>

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class ScaleWithScreenSizeProcessor : InputProcessor<Vector2>
    {
#if UNITY_EDITOR
        static ScaleWithScreenSizeProcessor()
        {
            Initialise();
        }
#endif

        [RuntimeInitializeOnLoadMethod]
        static void Initialise()
        {
            InputSystem.RegisterProcessor<ScaleWithScreenSizeProcessor>();
        }

        [Tooltip("If true, scales with screen height. If false, scales with screen width.")]
        public bool m_useHeight;

        public override Vector2 Process(Vector2 value, InputControl control)
        {
            return value * (m_useHeight ? Screen.height : Screen.width) / 200f;
        }
    }
}
