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
                    Sound.SFXController.Instance.PlayRandomSoundClip("Footsteps", transform.parent.parent);
            }

            public void EnableMovementPickup()
            {
                GameManager.Player.Controller.SetCanMove(CanMoveType.Pickup, true);
            }

            public void DisableMovementForPickup()
            {
                if (GameManager.Player.Interaction.heldObject)
                    GameManager.Player.Controller.SetCanMove(CanMoveType.Pickup, false);
            }

            public void EnableHolding()
            {
                GameManager.Player.Animation.PassBoolParameter("IsHolding", true);
            }
        }
    }
}
