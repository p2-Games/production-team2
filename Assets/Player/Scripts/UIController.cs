///<summary>
/// Author: EMily
///
///
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
                if (context.started && UIMenuManager.Instance.activeMenus.Count < 2)
                {
                    GameManager.Instance.PauseGame();
                }
            }
        }

	}

}
