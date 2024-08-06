///<summary>
/// Author: Brayden
///
/// Making event trigger an animation like moving doors.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class PlayAnimationEvent : Event
            {
                [Tooltip ("Drag in animator")]
                [SerializeField] Animator[] m_animator;

                public override void DoEvent(bool value)
                {
                   //play animation
                }
            }

        }
    }
}