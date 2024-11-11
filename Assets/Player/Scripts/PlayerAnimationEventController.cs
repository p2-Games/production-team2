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

            public void ToggleMovementForPickup()
            {
                GameManager.Player.Controller.SetCanMove(CanMoveType.Pickup, !GameManager.Player.Controller.GetCanMove(CanMoveType.Pickup));
            }

            public void TriggerHolding()
            {
                GameManager.Player.Animation.PassBoolParameter("IsHolding", !GameManager.Player.Animation.GetBoolParameter("IsHolding"));
            }
        }
    }
}
