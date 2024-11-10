///<summary>
/// Author: Emily
///
/// Detects the pause button being pressed and calls GameManager to pause the game
///
///</summary>

using Millivolt.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        public class UIController : MonoBehaviour
	    {
            public void Pause(InputAction.CallbackContext context)
            {
                if (context.started)
                {
                    // If no menus are open then Pause the game
                    if (UIMenuManager.Instance.activeMenus.Count == 0)
                    {
                        GameManager.Instance.PauseGame();
                        return;
                    }


                    // Check if the pause menu is the current active menu, if so Unpause the game
                    if (UIMenuManager.Instance.activeMenus[0].GetComponent<PauseMenu>() != null)
                    {
                        GameManager.Instance.PauseGame();
                    }
                    // Otherwise deactivate the currently open menu
                    else 
                    {
                        UIMenuManager.Instance.activeMenus[0].DeactivateMenu();
                    }
                }
            }
        }

	}

}
