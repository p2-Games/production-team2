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

			bool test = true;

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

            private void Start()
            {
				Invoke("ActivateDeactivateTest", 5f);
				Invoke("ActivateDeactivateTest", 10f);
            }

			private void ActivateDeactivateTest()
			{
				if (test)
				{
					m_sceneMenus[m_currentMenu].ActivateMenu();
					test = false;
				}
				else
				{
                    m_sceneMenus[m_currentMenu].DeactivateMenu();
                }

			}
        }
	}
}
