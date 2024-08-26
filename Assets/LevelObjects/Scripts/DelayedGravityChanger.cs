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
            public class DelayedGravityChanger : GravityChanger
            {
                public override bool isActive
                {
                    get => base.isActive;
                    set
                    {
                        if (!value)
                            m_gravityUI.StopAllCoroutines();

                        m_isActive = value;
                    }
                }

                [Header("Delayed Gravity Details"), SerializeField] private GravityIndicatorUI m_gravityUI;

                [Tooltip("How long it will take before gravity changes if change instantaneously is false.")]
                [SerializeField] private float m_delay;

                private float m_timer;
                private bool m_flipUp = false;

                public void ChangeGravityAfterTime(bool value)
                {
                    float indicatorFlashInterval = m_delay / 8f;
                    m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(indicatorFlashInterval));
                    m_timer = m_delay;
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
