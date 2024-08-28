///<summary>
/// Author: Halen
///
/// Base class for level objects that use UnityEvents.
///
///</summary>

using UnityEngine;
using UnityEngine.Events;

namespace Millivolt
{
	namespace LevelObjects
	{
		public abstract class EventObject : LevelObject
		{
            [Tooltip("Events that occur when the object is enabled. Trigger Objects always call these events.")]
            [SerializeField] private UnityEvent<bool> m_activateEvents;
		}
	}
}
