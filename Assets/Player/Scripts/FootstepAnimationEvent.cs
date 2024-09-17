///<summary>
/// Author:
///
///
///
///</summary>

using Millivolt.Sound;
using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        public class FootstepAnimationEvent : MonoBehaviour
        {
            public void Play()
            {
                SFXController.Instance.PlayRandomSoundClipObject("Footsteps", transform.parent.parent);
                print("step");
            }
        }
    }
}
