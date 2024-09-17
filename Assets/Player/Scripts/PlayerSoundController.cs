///<summary>
/// Author: Halen
///
/// Used for controlling the sound effects of the Player to keep them separate from the other scripts and avoid clutter.
///
///</summary>

using System.Collections;
using UnityEngine;

namespace Millivolt
{
    using Sound;

	namespace Player
	{
		public class PlayerSoundController : MonoBehaviour
		{
            public void Jump()
            {
                SFXController.Instance.PlayRandomSoundClipObject("Footsteps", transform.parent);
            }

            public void Land()
            {
                SFXController.Instance.PlayRandomSoundClipObject("Footsteps", transform.parent);
            }
        }
	}
}
