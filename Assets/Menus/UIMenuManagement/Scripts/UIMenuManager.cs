///<summary>
/// Author: Emily
///
/// A Manager for handling all menus in a scene
///
///</summary>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	namespace UI
	{
		public class UIMenuManager : MonoBehaviour
		{
			public static UIMenuManager Instance { get; private set; }
			private void Awake()
            {
				if (!Instance)
					Instance = this;
				else if (Instance != this)
					Destroy(gameObject);

				DontDestroyOnLoad(gameObject);
            }

			public List<UIMenu> activeMenus;

			/// <summary>
			/// This will update the status of UIMenu objects in the activeMenu's list
			/// </summary>
			/// <param name="menuIndex"></param>
			public void SetActiveMenu()
			{
				for (int i = 0; i < activeMenus.Count; i++)
				{
					if (i != 0)
					{
						activeMenus[i].GetComponent<GraphicRaycaster>().enabled = false;
						if (activeMenus[i].hideOnInactive)
							activeMenus[i].gameObject.SetActive(false);
					}
					else
						activeMenus[0].GetComponent<GraphicRaycaster>().enabled = true; activeMenus[0].gameObject.SetActive(true); 
                }
			}

            private void Start()
            {
				//Check if the scene starts with a menu open and add it to the active scenes list
				//FindObjectOfType<UIMenu>().ActivateMenu();
            }
        }
	}
}
