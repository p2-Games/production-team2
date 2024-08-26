///<summary>
/// Author: Halen
///
/// For Level Objects that trigger Events.
///
///</summary>

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Millivolt
{
	namespace LevelObjects
	{
		public abstract class EventObject : LevelObject
		{
            public override bool isActive
            {
                get => base.isActive;
                set
                {
                    foreach (UnityEvent<bool> _event in value ? m_activateEvents : m_deactivateEvents)
                        _event.Invoke(value);

                    m_isActive = value;
                }
            }

            [Tooltip("The events that will occur when the object's active state is changed to active.")]
            [SerializeField] protected UnityEvent<bool>[] m_activateEvents;

            [Tooltip("The events that will occur when the object's active state is changed to inactive.")]
            [SerializeField] protected UnityEvent<bool>[] m_deactivateEvents;
        }
	}
}
