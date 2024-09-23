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
                SFXController.Instance.PlayRandomSoundClip("Footsteps", transform.parent.parent);
            }
        }
    }
}
