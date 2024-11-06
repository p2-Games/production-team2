///<summary>
/// Author: Halen
///
/// Script specifcally for playing sound effects using Animation Events for the player.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerAnimationEventController : MonoBehaviour
        {
            public void PlayFootstep()
            {
                if (GameManager.Player.Controller.isGrounded)
                    SFXController.Instance.PlayRandomSoundClip("Footsteps", transform.parent.parent);
            }

            public void DisableMovementForPickup()
            {
                GameManager.Player.Controller.SetCanMove(false, CanMoveType.Pickup);
            }

            public void EnableMovementForPickup()
            {
                GameManager.Player.Controller.SetCanMove(true, CanMoveType.Pickup);
            }
        }
    }
}
