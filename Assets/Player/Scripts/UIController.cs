///<summary>
/// Author: EMily
///
///
///
///</summary>

using Millivolt.Utilities;
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
                    GameManager.Instance.PauseGame();
                }
            }
        }

	}

}
