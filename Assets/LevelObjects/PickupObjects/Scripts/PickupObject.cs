///<summary>
/// Author: Halen
///
/// Script for indentifying objects that the player can pick up.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace PickupObjects
        {
            public enum PickupType
            {
                Standard,
                Immovable
            }

            [RequireComponent(typeof(Rigidbody))]
            public class PickupObject : LevelObject
            {
                [Header("Pickup Object Details")]
                [SerializeField] protected PickupType m_type;

                [SerializeField] protected float m_useTime;
                protected float m_timer;

                protected bool m_inUse = false;
                public bool inUse => m_inUse;

                public PickupType pickupType => m_type;
                public bool playerCanGrab => m_type != PickupType.Immovable;

                protected Rigidbody m_rb;
                public Rigidbody rb => m_rb;

                protected virtual void Start()
                {
                    m_rb = GetComponent<Rigidbody>();
                    m_timer = 0;
                }

                protected virtual void Update()
                {
                    if (m_timer < m_useTime)
                        m_timer += Time.deltaTime;
                }

                public virtual void Use()
                {
                    if (m_timer < m_useTime)
                        return;

                    m_inUse = !m_inUse;

                    m_timer = 0;
                }

                private void OnCollisionEnter(Collision collision)
                {
                    SFXController.Instance.PlayRandomSoundClip("ScrewDrop", transform);
                }
            }
        }
    }
}