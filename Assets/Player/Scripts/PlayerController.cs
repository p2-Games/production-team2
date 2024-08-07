///<summary>
/// Author: Halen Finlay
///
/// Handles the player physics, movement, jumping, and camera logic.
/// 
///</summary>

using Cinemachine;
using Pixelplacement.TweenSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        [RequireComponent(typeof(CapsuleCollider), typeof(PlayerInput))]
        public class PlayerController : MonoBehaviour
        {
            void Start()
            {
                InitialiseRigidbody();
                InitialiseCollider();
            }

            private void FixedUpdate()
            {
                MovePlayer();
            }

            [Header("Physics")]
            [Tooltip("The acceleration of gravity of the player, on the player's transform.up axis.")]
            [SerializeField] private float m_gravity;
            [Tooltip("The layers of objects that the CharacterController can interact with.")]
            [SerializeField] private LayerMask m_walkableLayers;

            public float height => m_collider.height;

            private Rigidbody m_rb;
            private CapsuleCollider m_collider;

            public float gravity => m_gravity;

            [ContextMenu("Initialise Rigidbody")]
            private void InitialiseRigidbody()
            {
                m_rb = GetComponentInParent<Rigidbody>();
            }

            [ContextMenu("Initialise Collider")]
            private void InitialiseCollider()
            {
                m_collider = GetComponent<CapsuleCollider>();

                PhysicMaterial pm = new PhysicMaterial();
                pm.frictionCombine = PhysicMaterialCombine.Minimum;
                pm.staticFriction = 0f;
                pm.dynamicFriction = 0f;
                pm.bounceCombine = PhysicMaterialCombine.Minimum;
                pm.bounciness = 0f;

                m_collider.material = pm;
            }

            [Header("Movement")]
            [Tooltip("The movement speed of the player in units per second.")]
            [SerializeField] private float m_topSpeed;
            [SerializeField] private float m_acceleration;
            [SerializeField] private float m_decceleration;
            [SerializeField, Range(0, 90)] private float m_slopeLimit;

            private Vector2 m_moveDirection;
            private Vector3 m_walkVelocity;
            private Vector3 m_externalVelocity;
            private Vector3 m_constantExternalVelocity;
            private float m_verticalVelocity;

            public void Move(InputAction.CallbackContext context)
            {
                m_moveDirection = context.ReadValue<Vector2>();
            }

            /// <summary>
            /// Calculate how much the player should move this frame and apply it.
            /// </summary>
            private void MovePlayer()
            {
                // only do the boxcast once per frame
                m_isGrounded = isGrounded;

                // get camera directions
                Vector3 camRight = Camera.main.transform.right;
                Vector3 camForward = Camera.main.transform.forward;

                // 'flatten' based on player transform
                camRight = Vector3.Project(camRight, transform.right);
                camForward = Vector3.Project(camForward, transform.forward);

                // re-normalise now value has been edited
                camRight = camRight.normalized;
                camForward = camForward.normalized;

                // apply direction and speed
                Vector3 horizontalRelativeInput = m_moveDirection.x * camRight;
                Vector3 verticalRelativeInput = m_moveDirection.y * camForward;

                // calc movement vector
                Vector3 targetVelocity = (horizontalRelativeInput + verticalRelativeInput) * m_topSpeed;

                // calculate velocity change vector
                if (m_constantExternalVelocity == Vector3.zero)
                    m_walkVelocity = Vector3.MoveTowards(m_walkVelocity, targetVelocity, (m_moveDirection == Vector2.zero ? m_decceleration : m_acceleration) * Time.fixedDeltaTime);
                else
                    m_walkVelocity = Vector3.zero;

                // only apply gravity if not grounded
                if (!m_isGrounded)
                    m_verticalVelocity += m_gravity * Time.fixedDeltaTime;

                // if the player should jump, add the jump velocity
                if (m_willJump)
                {
                    m_verticalVelocity += m_jumpSpeed;
                    m_willJump = false;
                }

                // move player
                m_rb.velocity = m_walkVelocity + m_verticalVelocity * transform.up + m_externalVelocity + m_constantExternalVelocity;
            }

            public void SetExternalVelocity(Vector3 value)
            {
                m_constantExternalVelocity = value;
                m_constantExternalVelocity.y = 0;
                m_verticalVelocity = value.y;
            }

            [Header("Jumping")]
            [Tooltip("The velocity added to the player in units per second when they jump.")]
            [SerializeField] private float m_jumpSpeed;
            [Tooltip("Distance below bottom of player for grounded check.")]
            [SerializeField, Range(0,0.5f)] private float m_groundCheckDistance;
            [Tooltip("The radius of the grounded check.")]
            [SerializeField, Range(0,1)] private float m_groundCheckRadius;

            private bool m_isGrounded;

            private bool m_willJump = false;

            /// <summary>
            /// <summary>
            /// Make the player jump next time MovePlayer is called.
            /// </summary>
            public void Jump(InputAction.CallbackContext context)
            {
                if (context.started && m_isGrounded)
                {
                    m_willJump = true;
                }
            }

            /// Returns if the player is standing on a Walkable collider.
            /// </summary>
            public bool isGrounded
            {
                get
                {
                    // check the space underneath the player to determine if grounded
                    // if there is no walkable object under the player, they are not grounded
                    if (!Physics.BoxCast(transform.position + m_collider.center, new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        -transform.up, out RaycastHit hit, transform.rotation, m_collider.height / 2, m_walkableLayers, QueryTriggerInteraction.Ignore))
                        return false;
                    // if the player is on a walkable object
                    else
                    {
                        // if the object does not meet the slope limit requirements, then the player is NOT standing on it
                        // STRETCH: snap to slopes
                        if (Vector3.Angle(hit.normal, transform.up) > m_slopeLimit)
                            return false;

                        // if the hit object has a rigidbody, apply its velocity to the player.
                        if (hit.rigidbody)
                        {
                            m_externalVelocity = hit.rigidbody.GetPointVelocity(hit.point);
                        }

                        // TODO: reduce over time instead of set to zero
                        else
                        {
                            if (m_externalVelocity != Vector3.zero)
                                m_externalVelocity = Vector3.zero;

                            // float currentLength = m_externalVelocity.magnitude;
                            // m_externalVelocity.Normalize();
                            // m_externalVelocity *= currentLength / m_decceleration;
                        }

                        return true;
                    }

                    // ALTERNATE METHOD: checkbox
                    //return Physics.CheckBox(new Vector3(transform.position.x, transform.position.y - m_collider.height / 2, transform.position.z), 
                    //   new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius), Quaternion.identity, m_walkableLayers);
                }
            }

            public bool hittingHead
            {
                get
                {
                    return Physics.BoxCast(transform.position + m_collider.center, new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        transform.up, out RaycastHit hit, transform.rotation, m_collider.height / 2, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore);
                }
            }

            private void OnCollisionEnter(Collision collision)
            {
                if (hittingHead)
                    m_verticalVelocity = 0;
                
                // give control back to player
                if (m_constantExternalVelocity != Vector3.zero)
                    m_constantExternalVelocity = Vector3.zero;
            }

            private void OnCollisionStay(Collision collision)
            {
                // if the colliding game object's layer is a walkable layer AND the player is currently grounded, meaning the player is standing on that object
                if ((m_walkableLayers & (1 << collision.gameObject.layer)) != 0 && m_isGrounded)
                {
                    // if player is moving dowwnards
                    if (m_verticalVelocity < 0)
                        m_verticalVelocity = 0;
                }
            }            

#if UNITY_EDITOR
            [Header("Debug"), SerializeField] private bool m_drawGizmos;
            private void OnDrawGizmos()
            {
                if (!m_drawGizmos)
                    return;

                if (m_collider)
                {
                    Handles.color = Color.green;
                    Handles.DrawWireCube(transform.position + m_collider.center - transform.up * m_collider.height / 2,
                        new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius));

                    Handles.color = Color.cyan;
                    Handles.DrawWireCube(transform.position + m_collider.center + transform.up * m_collider.height / 2,
                        new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius));
                }
            }
#endif
        }
    }
}
