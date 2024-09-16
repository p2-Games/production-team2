///<summary>
/// Author: Halen Finlay
///
/// Handles the player physics, movement, jumping, and camera logic.
/// 
///</summary>

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        [RequireComponent(typeof(CapsuleCollider), typeof(Rigidbody), typeof(PlayerInput))]
        public class PlayerController : MonoBehaviour
        {
            void Start()
            {
                InitialiseRigidbody();
                InitialiseCollider();
                InitialiseTargetDirection();
                m_animation = GetComponent<AnimationController>();
            }

            private void FixedUpdate()
            {
                MovePlayer();
            }

            private AnimationController m_animation;

            [Header("Physics")]
            [Tooltip("The layers of objects that the CharacterController can interact with.")]
            [SerializeField] private LayerMask m_walkableLayers;

            private Rigidbody m_rb;
            private CapsuleCollider m_collider;

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

            public bool canMove = true;

            private Vector2 m_moveInput;
            private Vector3 m_walkVelocity;
            private Vector3 m_externalVelocity;
            private Vector3 m_platformVelocity;

            private Vector3 m_verticalVelocity;

            private Vector3 m_surfaceNormal;

            public void Move(InputAction.CallbackContext context)
            {
                m_moveInput = context.ReadValue<Vector2>();
            }

            /// <summary>
            /// Calculate how much the player should move this frame and apply it.
            /// </summary>
            private void MovePlayer()
            {
                // only do the boxcast once per physics frame
                m_isGrounded = isGrounded;

                // project camera direction onto player direction for relative movement
                Vector3 camRight = Vector3.ProjectOnPlane(Camera.main.transform.right, transform.up);
                Vector3 camForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, transform.up);

                // normalise value
                camRight = camRight.normalized;
                camForward = camForward.normalized;

                // clamp move input
                m_moveInput = Vector3.ClampMagnitude(m_moveInput, 1f);

                // apply input value
                Vector3 horizontalRelativeInput = m_moveInput.x * camRight;
                Vector3 verticalRelativeInput = m_moveInput.y * camForward;

                // combine and apply movement speed
                Vector3 targetVelocity = (horizontalRelativeInput + verticalRelativeInput) * m_topSpeed;

                // calculate velocity change vector
                if (canMove)
                    m_walkVelocity = Vector3.MoveTowards(m_walkVelocity, targetVelocity, (m_moveInput == Vector2.zero ? m_decceleration : m_acceleration) * Time.fixedDeltaTime);
                else
                    m_walkVelocity = Vector3.zero;

                // project onto surface the player is currently standing on
                if (m_surfaceNormal != Vector3.zero)
                    m_walkVelocity = Vector3.ProjectOnPlane(m_walkVelocity, m_surfaceNormal)/*.normalized * m_walkVelocity.magnitude*/;

                // only apply gravity if not grounded
                if (!m_isGrounded)
                    m_verticalVelocity += Physics.gravity * Time.fixedDeltaTime;

                if (!canMove && m_willJump)
                    m_willJump = false;

                // if the player should jump, add the jump velocity
                if (m_willJump)
                {
                    m_verticalVelocity += m_jumpSpeed * transform.up;
                    m_willJump = false;
                }

                // tell animator what to do
                m_animation.PassFloatParameter("Speed", m_walkVelocity.magnitude / m_topSpeed);

                // move player
                m_rb.velocity = m_walkVelocity + m_verticalVelocity + m_platformVelocity + m_externalVelocity;
            }

            /// <summary>
            /// Adds an amount to the player's vertical velocity.
            /// </summary>
            /// <param name="value">The amount to add.</param>
            public void AddVerticalVelocity(Vector3 value) => m_verticalVelocity += value;

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
            public void SetExternalVelocity(Vector3 value)
            {
                m_rb.velocity = Vector3.zero;
                m_verticalVelocity = Vector3.zero;
                m_externalVelocity = value;
            }
            [Header("Heading")]
            [SerializeField] private float m_forwardChangeSpeed = 0.3f;
            [SerializeField] private float m_upChangeSpeed = 0.5f;

            private Vector3 m_targetForward;
            private Vector3 m_targetUp;

            private void InitialiseTargetDirection()
            {
                m_targetForward = transform.forward;
                m_targetUp = transform.up;
            }

            public void SetTargetForward(Vector3 value) => m_targetForward = value;

            private void Update()
            {
                // move player to face correct direction when move direction is not zero
                m_targetUp = Vector3.MoveTowards(m_targetUp, -Physics.gravity.normalized, m_forwardChangeSpeed);
                if (m_walkVelocity != Vector3.zero)
                    m_targetForward = Vector3.MoveTowards(m_targetForward, Vector3.ProjectOnPlane(m_walkVelocity, m_targetUp).normalized, m_upChangeSpeed);

                transform.rotation = Quaternion.LookRotation(m_targetForward, m_targetUp);
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
                    RaycastHit[] hits = Physics.BoxCastAll(transform.position + m_collider.center,
                        new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        -transform.up, transform.rotation, m_collider.height / 2, m_walkableLayers, QueryTriggerInteraction.Ignore);

                    // if no hits, the player is not standing on anything
                    if (hits.Length == 0)
                    {
                        m_surfaceNormal = Vector3.zero;
                        return false;
                    }
                    // if the player is on a walkable object
                    else
                    {
                        // if the player is changing from not grounded to grounded aka landing,
                        // reset their various velocities to 0
                        if (!m_isGrounded)
                        {
                            m_verticalVelocity = Vector3.zero;
                            m_platformVelocity = Vector3.zero;
                            if (m_externalVelocity != Vector3.zero)
                            {
                                m_externalVelocity = Vector3.zero;
                                canMove = true;
                            }
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
                        if (Vector3.Angle(hit.normal, transform.up) > m_slopeLimit)
                            return false;

                        // save the normal of the surface the player is standing on
                        m_surfaceNormal = hit.normal;

                        return true;
                    }

                    // ALTERNATE METHOD: checkbox
                    //return Physics.CheckBox(new Vector3(transform.position.x, transform.position.y - m_collider.height / 2, transform.position.z), 
                    //   new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius), Quaternion.identity, m_walkableLayers);
                }
            }

            /// <summary>
            /// Reset all the functions of the player and gravity
            /// </summary>
            public void ResetPlayer()
            {
                m_externalVelocity = Vector3.zero;
                m_verticalVelocity = Vector3.zero;
                m_platformVelocity = Vector3.zero;

                GameManager.Instance.ChangeGravity(LevelManager.Instance.levelData.gravityDirection, LevelManager.Instance.levelData.gravityMagnitude);
            }

            public bool hittingHead
            {
                get
                {
                    return Physics.BoxCast(transform.position + m_collider.center, new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        transform.up, /*out RaycastHit hit,*/ transform.rotation, m_collider.height / 2, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore);
                }
            }

            private void OnCollisionEnter(Collision collision)
            {
                // check if the player is hitting their head on the ceiling
                if (hittingHead && m_verticalVelocity.sqrMagnitude > 0)
                    m_verticalVelocity = Vector3.zero;
            }

#if UNITY_EDITOR
            [Header("Debug"), SerializeField] private bool m_drawGizmos;
            private void OnDrawGizmos()
            {
                if (!m_drawGizmos)
                    return;

                if (!m_collider)
                    InitialiseCollider();

                Handles.matrix = transform.localToWorldMatrix;

                Handles.color = Color.green;
                Handles.DrawWireCube(m_collider.center - Vector3.up * m_collider.height / 2,
                    new Vector3(m_groundCheckRadius * 2, m_groundCheckDistance * 2, m_groundCheckRadius * 2));

                Handles.color = Color.cyan;
                Handles.DrawWireCube(m_collider.center + Vector3.up * m_collider.height / 2,
                    new Vector3(m_groundCheckRadius * 2, m_groundCheckDistance * 2, m_groundCheckRadius * 2));

                Handles.color = Color.magenta;
                Handles.ArrowHandleCap(0, m_collider.center - Vector3.up * m_collider.height / 2,
                                        Quaternion.LookRotation(Vector3.down, (Physics.gravity.magnitude * -transform.up).normalized), 1, EventType.Repaint);
            }
#endif
        }
    }
}
