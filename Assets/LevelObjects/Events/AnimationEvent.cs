///<summary>
/// Author: Halen
///
/// Plays animations on event trigger.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class AnimationEvent : Event
            {
                [SerializeField] private Animator[] m_animators;

                public override void DoEvent(bool value)
                {
                    foreach (Animator anim in m_animators)
                    {
                        if (value)
                            anim.Play("OnActivate");
                        else
                            anim.Play("OnDeactivate");
                    }    
                }
            }
        }
    }
}
