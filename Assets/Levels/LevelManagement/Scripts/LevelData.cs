///<summary>
/// Author: Emily
///
/// ScriptableObject for LevelData
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    [CreateAssetMenu(menuName = "ScriptableObject/LevelData", fileName = "LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        public float defaultGravityMagnitude;
        public Vector3 defaultGravityDirection;
    }
}
