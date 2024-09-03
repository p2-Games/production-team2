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
            [SerializeField] protected bool m_isActive;

            /// <summary>
            /// Used for changing the active state of the Level Object.
            /// </summary>
            /// <returns>The active state of the Level Object.</returns>
            public virtual bool isActive
            {
                get => m_isActive;
                set => m_isActive = value;
            }

            [SerializeField] protected string m_name;
			new public string name => m_name;
            [SerializeField, TextArea] protected string m_description = "";

            [SerializeField] protected bool m_drawGizmos = true;

			// spawn parent, if it exists
			[HideInInspector] public ObjectSpawner spawnParent;
        }
	}
}
