///<summary>
/// Author: Halen
///
/// Base class for events for Level Objects
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace LevelObjects
	{
		namespace Events
		{
			public abstract class Event : MonoBehaviour
			{
				public abstract void DoEvent(bool value);
			}
		}
	}
}
