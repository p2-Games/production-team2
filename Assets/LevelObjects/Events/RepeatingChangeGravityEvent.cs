///<summary>
/// Author:
///
/// Repeatedly changes players transform.up through a list of Vector3
/// Works as a toggle so the same event will start/stop the repeating graviuty switching
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
            public class RepeatingChangeGravityEvent : Event
            {
                [SerializeField] private GravityIndicatorUI m_gravityUI;
                [Tooltip("How long it will take before the actual gravity switches")]
                [SerializeField] private float m_gravSwitchTime;
                [Tooltip("List of transforms that the function will repeat through in order")]
                [SerializeField] private Vector3[] m_gravitySets;
                [Tooltip("How long it will take before the function repeats")]
                [SerializeField] private float m_repeatTime;
                [SerializeField] private int m_currentGravity;

                [Tooltip("An initial delay before the repeating function starts")]
                [SerializeField] private float m_initialDelay;

                private Coroutine m_uiFlashing;

                private bool isRepeating;

                /// <summary>
                /// Gravity is just rotation of the player so use this function to set the players rotation
                /// By Default gravity is set at 0
                /// </summary>
                /// <param name="newGravity"></param>
                public void ChangeGravity()
                {
                    if (isRepeating)
                    {
                        m_currentGravity++;
                        if (m_currentGravity > m_gravitySets.Length - 1)
                        {
                            m_currentGravity = 0;
                        }
                        float indicatorFlashInterval = (m_gravSwitchTime / 5);
                        m_uiFlashing = m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(indicatorFlashInterval, m_gravitySets[m_currentGravity]));
                    }
                }

                public override void DoEvent(bool value)
                {
                    if (value)
                    {
                        if (!isRepeating)
                        {
                            isRepeating = true;
                            InvokeRepeating("ChangeGravity", m_initialDelay, m_gravSwitchTime + m_repeatTime);                        
                        }
                        else
                        {
                            StopRepeating();
                        }
                    }
                }

                /// <summary>
                /// Cancel the gravity switch and set gravity back to standard
                /// </summary>
                private void StopRepeating()
                {
                    isRepeating = false;
                    StopCoroutine(m_uiFlashing);
                    CancelInvoke();
                    m_currentGravity = 0;
                    m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(0, new Vector3()));
                }
            }
        }
    }
}
