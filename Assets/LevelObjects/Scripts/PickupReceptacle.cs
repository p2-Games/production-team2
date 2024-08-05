///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        using Events;
        public class PickupReceptacle : LevelObject
        {
            public override bool isActive
            {
                get => base.isActive;
                set
                {
                    m_isActive = value;

                    foreach (Event eve in m_events)
                        eve.DoEvent(m_isActive);

                    if (m_isActive)
                        m_animator.Play("OnActive");
                    else
                        m_animator.Play("OnInactive");
                }
            }
            [Tooltip("The Events that trigger when the active state of this Level Object changes.")]
            [SerializeField] private Event[] m_events;

            private Animator m_animator;

            private void Start()
            {
                m_animator = GetComponent<Animator>();
            }
        }
    }
}
