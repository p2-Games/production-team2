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
					{
                        activeMenus[0].GetComponent<GraphicRaycaster>().enabled = true; activeMenus[0].gameObject.SetActive(true);
                        if (activeMenus[i].interactable)
                        {
                            EventSystemManager.Instance.SetCurrentSelectedGameObject(activeMenus[i].firstSelected);
                        }
                    }
                }
			}

            private void Start()
            {
				CursorLockupdate();
            }

			public void CursorLockupdate()
			{
				if (activeMenus.Count > 0)
				{
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
				else
				{
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
			}

            public void ClearActiveMenus()
            {
				activeMenus.Clear();
            }
        }
	}
}
