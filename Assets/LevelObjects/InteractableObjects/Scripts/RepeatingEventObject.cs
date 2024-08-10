///<summary>
/// Author: Halen
///
/// Like a pressable button, but instead of triggering its events when the active state changes,
/// instead toggles them every set number of seconds.
///
///</summary>

using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        using Events;

        public class RepeatingEventObject : InteractableObject
        {
            public override bool isActive
            {
                get => m_isActive;
                set
                {
                    m_isActive = value;

                    if (m_isActive)
                    {
                        if (m_activateImmediately)
                            ToggleEvents(m_firstPulse);
                        else
                        {
                            m_repeatingTimer = m_repeatTime;
                            m_lastPulse = m_firstPulse;
                        }
                    }
                    else
                        ToggleEvents(!m_firstPulse);
                }
            }

            [Header("Repeating Object Details"), Tooltip("How often the object toggles it's events while it is active.")]
            [SerializeField] private float m_repeatTime;

            [Tooltip("If the timer should take a pass first when this object is activated before events are toggled,\n" +
                "or if when the object is activated the Events are all toggled immediately.")]
            [SerializeField] private bool m_activateImmediately;

            [Tooltip("What the value of the first activation should be.\n" +
                "ie, if 'true', all events will be set to true on the first pulse, then false on the next, etc.")]
            [SerializeField] private bool m_firstPulse;

            private float m_repeatingTimer;

            private bool m_lastPulse;

            private void Start()
            {
                m_lastPulse = m_firstPulse;
                if (m_isActive && m_activateImmediately)
                    ToggleEvents(m_lastPulse);
            }

            new private void Update()
            {
                if (m_isActive)
                {
                    if (m_repeatingTimer < 0)
                        ToggleEvents(m_lastPulse);
                    else
                        m_repeatingTimer -= Time.deltaTime;
                }
            }

            private void ToggleEvents(bool value)
            {
                foreach (Event eve in m_events)
                {
                    eve.DoEvent(value);
                }

                m_lastPulse = !m_lastPulse;
                m_repeatingTimer = m_repeatTime;
            }
        }
    }
}
