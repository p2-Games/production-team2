///<summary>
/// Author: Emily
///
/// A Manager for handling all menus in a scene
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public class UIMenuManager : MonoBehaviour
		{
			[SerializeField] private UIMenu[] m_sceneMenus;
			[SerializeField] private int m_currentMenu;

			/// <summary>
			/// This will call the activate function of the 
			/// </summary>
			/// <param name="menuIndex"></param>
			public void SetActiveMenu(int menuIndex)
			{
				m_sceneMenus[m_currentMenu].DeactivateMenu();
				m_sceneMenus[menuIndex].ActivateMenu();
				m_currentMenu = menuIndex;
			}
		}
	}
}
