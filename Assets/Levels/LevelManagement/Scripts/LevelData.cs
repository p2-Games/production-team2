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
        [SerializeField] private string m_levelName;
        [Space]
        [SerializeField] private float m_gravityMagnitude;
        [SerializeField] private Vector3 m_gravityDirection;


        public string levelName => m_levelName;
        public float gravityMagnitude => m_gravityMagnitude;
        public Vector3 gravityDirection => m_gravityDirection;
    }
}
