///<summary>
/// Author: Emily
///
/// A base class for any menu in the game, all menus will derive from this
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public abstract class UIMenu : MonoBehaviour
		{
			private bool m_isActive;
			public bool isActive
			{
				get { return m_isActive; }
				set { m_isActive = value; }
			}

			/// <summary>
			/// Call a tween with Surge to make the UI menu appear and set it's active state to true
			/// </summary>
			public virtual void ActivateMenu()
			{
			}

			/// <summary>
			/// Call a tween with Surge to make the UI menu dissapear and set the active state to false
			/// </summary>
            public virtual void DeactivateMenu()
            {
            }
        }
	}
}
