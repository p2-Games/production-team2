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
                    // If the player has finished the game then stop them from being able to pause or exit the menu
                    if (GameManager.Instance.gameState == GameState.FINISH)
                    {
                        return;
                    }


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

            public void Tasklist(InputAction.CallbackContext context)
            {
                if (context.started && GameManager.Instance.gameState != GameState.PAUSE)
                {
                    if (GameManager.Tasklist.canActivate)
                        GameManager.Tasklist.SetTaskListActive(!GameManager.Tasklist.menu.isActive);
                }
            }
        }

	}

}
