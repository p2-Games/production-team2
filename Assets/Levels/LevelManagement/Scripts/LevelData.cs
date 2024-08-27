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
        [SerializeField] private float m_gravityMagnitude;
        [SerializeField] private Vector3 m_gravityEulerDirection;

        public float gravityMagnitude => m_gravityMagnitude;
        public Vector3 gravityEulerDirection => m_gravityEulerDirection;
    }
}
