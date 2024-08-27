///<summary>
/// Author: Halen
///
/// Changes the direction of gravity after a delay.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    using Player.UI;

    namespace LevelObjects
    {
        namespace Events
        {
            public class ChangeGravityDelayedEvent : ChangeGravityEvent
            {
                [Header("Delayed Gravity Details"), SerializeField] private GravityIndicatorUI m_gravityUI;

                [Tooltip("How long it will take before gravity changes.")]
                [SerializeField] private float m_delay;

                private float m_timer;
                private bool m_flipToSetDirection;

                public void ChangeGravityAfterTime(bool flip)
                {
                    // enable and start indicator canvas
                    m_gravityUI.gameObject.SetActive(true);
                    m_gravityUI.StopAllCoroutines();
                    m_gravityUI.StartGravityFlash(m_delay);

                    // prep delayed activation
                    m_flipToSetDirection = value;
                    m_isActive = true;
                    m_timer = 0;
                }

                private void Update()
                {
                    if (m_timer < m_delay)
                        m_timer += Time.deltaTime;
                    else if (m_flipUp)
                    {
                        ChangeGravity(m_flipUp);
                        m_flipUp = !m_flipUp;
                    }
                }
            }
        }
    }
}
