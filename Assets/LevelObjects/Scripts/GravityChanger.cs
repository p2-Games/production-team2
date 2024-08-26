///<summary>
/// Author: Emily McDonald & Halen
///
/// Changes player up transform to a specific vector3 when event is called
///
///</summary>

using Millivolt.Player;
using Millivolt.Utilities;
using UnityEditor;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace Events
        {
            public class GravityChanger : LevelObject
            {
                [Header("Gravity"), Tooltip("The acceleration of gravity in units per second squared.")]
                [SerializeField] private float m_magnitude;

                [Tooltip("The direction of gravity when this event triggers.")]
                [SerializeField] private Vector3 m_direction;

                protected void ChangeGravity(bool value)
                {                 
                    GameObject.FindWithTag("Player").GetComponent<PlayerController>().SetGravity(m_magnitude, m_direction); 
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    Handles.color = Color.yellow;
                    //Vector3 direction = Quaternion.Euler(m_direction) * Vector3.up / 2f;
                    //Vector3 endPoint = transform.position + direction;
                    //Handles.DrawLine(transform.position, endPoint);
                    Handles.ArrowHandleCap(0, transform.position, Quaternion.Euler(m_direction) * Quaternion.FromToRotation(Vector3.forward, Vector3.down), 1.4f, EventType.Repaint);
                }
#endif
            }
        }
    }
}
