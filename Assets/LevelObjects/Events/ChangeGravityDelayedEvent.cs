///<summary>
/// Author: Halen
///
/// Changes the direction of gravity after a delay.
///
///</summary>

using Millivolt.UI;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class ChangeGravityDelayedEvent : ChangeGravityEvent
            {
                [Header("Delayed Gravity Details"), SerializeField] private GravityIndicatorUI m_gravityUI;

                [Tooltip("How long it will take before gravity changes if change instantaneously is false.")]
                [SerializeField] private float m_delay;

                private float m_timer;
                private bool m_willSwitch = false;

                private bool m_flipping;

                public override void DoEvent(bool value)
                {
                    ChangeGravityAfterTime(value);
                }

                public void ChangeGravityAfterTime(bool flip)
                {
                    float indicatorFlashInterval = (m_delay / 5);
                    m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(indicatorFlashInterval));
                    m_willSwitch = true;
                    m_flipping = flip;
                    m_timer = m_delay;
                }

                private void Update()
                {
                    if (m_timer > 0)
                        m_timer -= Time.deltaTime;
                    else if (m_willSwitch)
                    {
                        ChangeGravity(m_flipping);
                        m_willSwitch = false;
                    }
                }
            }
        }
    }
}
