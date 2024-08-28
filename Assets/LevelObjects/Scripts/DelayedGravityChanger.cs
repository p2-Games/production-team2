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
                [Header("Delayed Gravity Details"), SerializeField] private GravityIndicatorUI m_gravityUI;

                [Tooltip("How long it will take before gravity changes if change instantaneously is false.")]
                [SerializeField] private float m_delay;

                private float m_timer;
                private bool m_flipping;
                private bool m_isActive;

                public void ChangeGravityAfterTime(bool flip)
                {
                    m_gravityUI.gameObject.SetActive(true);
                    m_gravityUI.StartFlash(m_delay);

                    m_flipping = flip;
                    m_isActive = true;
                    m_timer = 0;
                }

                private void Start()
                {
                    m_gravityUI.gameObject.SetActive(false);
                }

                private void Update()
                {
                    if (!m_isActive)
                        return;

                    if (m_timer < m_delay)
                        m_timer += Time.deltaTime;
                    else
                    {
                        ChangeGravity(m_flipping);
                        m_isActive = false;
                    }
                }
            }
        }
    }
}
