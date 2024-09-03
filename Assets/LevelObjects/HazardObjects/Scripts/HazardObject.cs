///<summary>
/// Author: Halen
///
/// Base class for objects that can damage or kill the player.
///
///</summary>

using Millivolt.Player;
using UnityEditor;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace HazardObjects
        {
            public abstract class HazardObject : LevelObject
            {               
                [Header("Hazard Details"), Tooltip("The base damage that the hazard deals to the player.")]
                [SerializeField] protected float m_damage;

                protected void DealDamage(Player.PlayerStatus player, float value)
                {
                    player.TakeDamage(value);
                }


#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    PlayerRotationParent prp = player.GetComponentInParent<PlayerRotationParent>();

                    Vector3 closestPoint = gameObject.GetComponent<Collider>().ClosestPoint(player.transform.position);

                    Vector3 dir = (player.transform.position - closestPoint).normalized;
                    dir = Vector3.ProjectOnPlane(dir, player.transform.up).normalized;

                    Handles.color = Color.blue;
                    Handles.SphereHandleCap(0, gameObject.GetComponent<Collider>().ClosestPoint(player.transform.position), Quaternion.identity, 0.2f, EventType.Repaint);

                    Handles.color = Color.white;
                    Handles.ArrowHandleCap(0, gameObject.GetComponent<Collider>().ClosestPoint(player.transform.position), Quaternion.LookRotation(dir, new Vector3()), 1, EventType.Repaint);
                }
#endif
            }
        }
    }
}
