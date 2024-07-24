/// <summary>
/// Credit: Gregoryl from Unity
/// https://discussions.unity.com/t/cinemachine-virtual-camera-rotates-faster-depending-on-the-fps/893314
/// 
/// This processor scales the value by the inverse of deltaTime.
/// It's useful for time-normalizing framerate-sensitive inputs such as pointer delta.
/// 
/// </summary>
/// 

using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif

class DeltaTimeScaleProcessor : InputProcessor<Vector2>
{
    public override Vector2 Process(Vector2 value, InputControl control) => value / Time.unscaledDeltaTime;

#if UNITY_EDITOR
    static DeltaTimeScaleProcessor() => Initialize();
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize() => InputSystem.RegisterProcessor<DeltaTimeScaleProcessor>();
}
