///<summary>
/// Author: Halen
///
/// Sets the velocity of the player when they enter a trigger.
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
    using PlayerController = Player.PlayerController;

    namespace LevelObjects
    {
        namespace InteractableObjects
        {
            public class LaunchPad : ToggleObject
            {
                [Header("LaunchPad Details"), SerializeField] private Vector3 m_initialVelocity;
                [SerializeField] private float m_snapSpeed;
                [SerializeField] private float m_minDistanceToLaunch;

                [Header("Debug"), SerializeField] private bool m_drawLines;
                [SerializeField] private int m_debugPointsToDraw;
                [SerializeField] private float m_debugTimeBetweenPoints;

                private Rigidbody m_objectToLaunch;
                private Vector3 m_newObjectPosition;

                private PlayerController m_player;

                private void Start()
                {
                    m_player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                }

                protected override void Update()
                {
                    base.Update();
                    
                    if (m_objectToLaunch && Vector3.Distance(m_objectToLaunch.position, transform.position) < m_minDistanceToLaunch)
                    {
                        if (m_player.gameObject == m_objectToLaunch.gameObject)
                        {
                            m_player.SetExternalVelocity(m_initialVelocity);
                            m_player.canMove = false;
                        }
                        else
                            m_objectToLaunch.velocity = m_initialVelocity;

                        m_objectToLaunch = null;
                    }
                }

                private void FixedUpdate()
                {
                    if (m_objectToLaunch)
                    {
                        m_objectToLaunch.MovePosition(Vector3.MoveTowards(m_objectToLaunch.position, transform.position, m_snapSpeed * Time.fixedDeltaTime));
                    }
                }

                private void OnTriggerEnter(Collider other)
                {
                    if (!m_isActive)
                        return;
                    
                    // if launchpad isn't currently trying to launch something AND the object in the trigger is allowed
                    if (!m_objectToLaunch && CanTrigger(other.gameObject))
                    {
                        m_newObjectPosition = transform.position;
                        m_newObjectPosition.y += other.gameObject.GetComponent<Collider>().bounds.extents.y;
                        m_objectToLaunch = other.gameObject.GetComponent<Rigidbody>();
                    }
                }

#if UNITY_EDITOR
                private void OnDrawGizmos()
                {
                    if (!m_player)
                        m_player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

                    Vector3 lastPoint = transform.position;

                    Handles.color = Color.blue;

                    for (int p = 1; p < m_debugPointsToDraw; p++)
                    {
                        float t = p * m_debugTimeBetweenPoints;

                        Vector3 nextPoint = transform.position + m_initialVelocity * t + 0.5f * m_player.gravity * t * t;

                        if (Physics.Raycast(lastPoint, (nextPoint - lastPoint).normalized, out RaycastHit hit, Vector3.Distance(lastPoint, nextPoint), ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore))
                        {
                            if (m_drawLines)
                                Handles.DrawLine(lastPoint, hit.point);
                            Handles.color = Color.red;
                            Handles.DrawWireCube(hit.point, new Vector3(0.5f, 0.5f, 0.5f));
                            break;
                        }
                        else
                        {
                            if (m_drawLines)
                                Handles.DrawLine(lastPoint, nextPoint);
                            else
                                Handles.DrawWireCube(nextPoint, new Vector3(0.5f, 0.5f, 0.5f));
                        }
    ;
                        lastPoint = nextPoint;
                    }
                }
#endif
            }
        }
    }
}