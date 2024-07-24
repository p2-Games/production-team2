///<summary>
/// Author: Halen
///
/// Base class for any non-standard objects that appear within levels.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace LevelObjects
	{
		public abstract class LevelObject : MonoBehaviour
		{
			protected bool m_isActive;
			public bool isActive { get { return m_isActive; } }

			/// <summary>
			/// Used for changing the active state of the Level Object.
			/// </summary>
			/// <param name="value">The state to set the Level Object to.</param>
			/// <returns>If the active state of the Level Object was changed.</returns>
			public bool SetActive(bool value)
			{
				// if value is changed
				if (m_isActive != value)
				{
					m_isActive = value;
					return true;
				}
				return false;
			}
		}
	}
}
