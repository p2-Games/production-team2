///<summary>
/// Author: Halen
///
/// Base class for level objects that move.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        public abstract class MovingLevelObject : LevelObject
        {
            protected Rigidbody m_rb;

            [Tooltip("How fast the platform moves in units per second.")]
            [SerializeField] protected float m_movementSpeed;

            protected virtual void Start()
            {
                m_rb = GetComponent<Rigidbody>();
            }
        }
    }
}
