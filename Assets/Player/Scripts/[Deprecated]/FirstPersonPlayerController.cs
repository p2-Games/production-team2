///<summary>
/// Author: Halen Finlay
///
/// Handles the player physics, movement, jumping, and camera logic.
/// 
///</summary>

using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(PlayerInput))]
        public class FirstPersonPlayerController : MonoBehaviour
        {
            /*
            void Start()
            {
                InitialiseRigidbody();
                InitialiseCollider();
                m_cameraController = parent.GetComponent<FirstPersonCameraController>();
            }

            private void FixedUpdate()
            {
                MovePlayer();
            }

            private FirstPersonCameraController m_cameraController;

            [Header("Physics")]
            [Tooltip("The acceleration of gravity of the player, on the player's transform.up axis.")]
            [SerializeField] private float m_gravity;
            [Tooltip("The layers of objects that the CharacterController can interact with.")]
            [SerializeField] private LayerMask m_walkableLayers;

            private Rigidbody m_rb;
            private CapsuleCollider m_collider;

            public Transform parent => transform.parent;
            public float height => m_collider.height;
            public Vector3 gravity => Mathf.Abs(m_gravity) * (Quaternion.Euler(parent.eulerAngles) * Vector3.down);

            public void SetGravity(float magnitude, Vector3 eulerDirection)
            {
                // if the player is holding a heavy object, dont flip them
                PickupObject pickupObject = GetComponentInChildren<PlayerInteraction>().heldPickupObject;
                if (pickupObject && pickupObject.pickupType == PickupType.Heavy)
                    return;
                
                // move the parent to the location of the player and reset the player's position so the player is rotated correctly
                parent.position = transform.position;
                transform.localPosition = Vector3.zero;

                // save rotation before changing for camera details
                Quaternion priorRotation = parent.rotation; 

                // change orientation and gravity
                Vector3 targetDirection = Quaternion.Euler(eulerDirection) * Vector3.back;
                parent.rotation = Quaternion.FromToRotation(Vector3.up, targetDirection);

                // could also try:
                // Quaternion.LookRotation(Vector3.down, targetDirection);

                // set magnitude/value of gravity
                m_gravity = -Mathf.Abs(magnitude);

                // set rotation of camera
                m_cameraController.SetLookRotation(priorRotation);
            }

            [ContextMenu("Initialise Rigidbody")]
            private void InitialiseRigidbody()
            {
                m_rb = GetComponent<Rigidbody>();
                m_rb.mass = 1;
                m_rb.drag = 0;
                m_rb.angularDrag = 0;
                m_rb.interpolation = RigidbodyInterpolation.Interpolate;
                m_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                m_rb.constraints = RigidbodyConstraints.FreezeRotation;
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
            private Vector3 m_platformVelocity;

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
                if (m_externalVelocity == Vector3.zero)
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
                m_rb.velocity = m_walkVelocity + m_verticalVelocity * transform.up + m_platformVelocity + m_externalVelocity;
            }

            /// <summary>
            /// Set the velocity of the player to the velocity of the platform they are standing on.
            /// </summary>
            /// <param name="value"></param>
            public void SetPlatformVelocity(Vector3 value) => m_platformVelocity = value;

            /// <summary>
            /// Set the velocity of the player to a value.
            /// Takes control away from the player until they land back on the ground.
            /// </summary>
            /// <param name="value"></param>
            public void SetExternalVelocity(Vector3 value) => m_externalVelocity = value;

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
                    RaycastHit[] hits = Physics.BoxCastAll(transform.position + m_collider.center,
                        new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        -transform.up, transform.rotation, m_collider.height / 2, m_walkableLayers, QueryTriggerInteraction.Ignore);

                    // if no hits, the player is not standing on anything
                    if (hits.Length == 0)
                        return false;
                    // if the player is on a walkable object
                    else
                    {
                        // if the player is changing from not grounded to grounded aka landing,
                        // reset their various velocities to 0
                        if (!m_isGrounded)
                        {
                            m_verticalVelocity = 0;
                            m_platformVelocity = Vector3.zero;
                            m_externalVelocity = Vector3.zero;
                        }

                        // get closest hit walkable object
                        Vector3 playerFeet = transform.position + m_collider.center - transform.up * m_collider.height / 2;
                        RaycastHit hit = hits[0];
                        for (int h = 1; h < hits.Length; h++)
                        {
                            if (Vector3.Distance(hits[h].point, playerFeet) < Vector3.Distance(hit.point, playerFeet))
                                hit = hits[h];
                        }

                        // if the closest object does not meet the slope limit requirements, then the player is NOT standing on it
                        // TODO: snap to slopes
                        if (Vector3.Angle(hit.normal, transform.up) > m_slopeLimit)
                            return false;

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
                // check if the player is hitting their head on the ceiling
                if (hittingHead)
                    m_verticalVelocity = 0;
            }

#if UNITY_EDITOR
            [Header("Debug"), SerializeField] private bool m_drawGizmos;
            private void OnDrawGizmos()
            {
                if (!m_drawGizmos)
                    return;

                if (m_collider)
                {
                    Handles.matrix = transform.localToWorldMatrix;
                    Handles.color = Color.green;
                    Handles.DrawWireCube(m_collider.center - Vector3.up * m_collider.height / 2,
                        new Vector3(m_groundCheckRadius * 2, m_groundCheckDistance * 2, m_groundCheckRadius * 2));

                    Handles.color = Color.cyan;
                    Handles.DrawWireCube(m_collider.center + Vector3.up * m_collider.height / 2,
                        new Vector3(m_groundCheckRadius * 2, m_groundCheckDistance * 2, m_groundCheckRadius * 2));

                    Handles.color = Color.magenta;
                    Handles.ArrowHandleCap(0, m_collider.center - Vector3.up * m_collider.height / 2,
                                            Quaternion.LookRotation(Vector3.down, gravity.normalized), 1, EventType.Repaint);
                }
            }
#endif
            */
        }
    }
}
