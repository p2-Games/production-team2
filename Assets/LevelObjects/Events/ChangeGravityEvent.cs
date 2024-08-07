///<summary>
/// Author: Emily McDonald
///
/// Changes player up transform to a specific vector3 when event is called
///
///</summary>

using Millivolt.UI;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class ChangeGravityEvent : Event
            {
                [SerializeField] private GravityIndicatorUI m_gravityUI;
                [Tooltip("How long it will take before the actual gravity switches")]
                [SerializeField] private float m_gravSwitchTime;
                [Tooltip("What the player rotation will be transformed to")]
                [SerializeField] private Vector3 m_newGravity;

                /// <summary>
                /// Gravity is just rotation of the player so use this function to set the players rotation
                /// By Default gravity is set at 0
                /// </summary>
                /// <param name="newGravity"></param>
                public void ChangeGravity(Vector3 newGravity, float gravityChangeTime)
                {
                    float indicatorFlashInterval = (gravityChangeTime / 5);
                    m_gravityUI.StartCoroutine(m_gravityUI.GravityUIFlashing(indicatorFlashInterval, newGravity));
                }

                public override void DoEvent(bool value)
                {
                    if (value)
                    {
                        ChangeGravity(m_newGravity, m_gravSwitchTime);
                    }
                }
            }
        }
    }
}
