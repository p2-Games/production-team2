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
        [RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
        public class PlayerController : MonoBehaviour
        {
            private void Start()
            {
                m_canMove = new PlayerCanMove();
            }

            private void FixedUpdate()
            {
                MovePlayer();
            }

            private AnimationController m_animation;
            new public AnimationController animation
            {
                get
                {
                    if (!m_animation)
                        m_animation = GetComponent<AnimationController>();
                    return m_animation;
                }
            }

            private PlayerModel m_model;
            private PlayerModel model
            {
                get
                {
                    if (!m_model)
                        InitialiseModel();
                    return m_model;
                }
            }

            private PlayerCanMove m_canMove;
            public bool canMove { get { return m_canMove.canMove; } }
            public void SetCanMove(bool value, CanMoveType type)
            {
                m_canMove.SetCanMove(value, type);
            }

            [Header("Physics")]
            [Tooltip("The layers of objects that the CharacterController can interact with.")]
            [SerializeField] private LayerMask m_walkableLayers;

            private Rigidbody m_rb;
            private Rigidbody rb
            {
                get
                {
                    if (!m_rb)
                        InitialiseRigidbody();
                    return m_rb;
                }
            }
            new public CapsuleCollider collider { get { return model.collider; } }

            [ContextMenu("Initialise GameManager.PlayerModel/Collider")]
            private void InitialiseModel()
            {
                m_model = GetComponentInChildren<PlayerModel>();
                m_model.InitialiseCollider();
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

            [Header("Movement")]
            [Tooltip("The movement speed of the player in units per second.")]
            [SerializeField] private float m_topSpeed;
            [SerializeField] private float m_acceleration;
            [SerializeField] private float m_decceleration;
            [SerializeField, Range(0, 90)] private float m_slopeLimit;

            private Vector2 m_moveInput;
            private Vector3 m_walkVelocity;
            private Vector3 m_verticalVelocity;
            private Vector3 m_externalVelocity;
            private Vector3 m_platformVelocity;

            private Vector3 m_surfaceNormal;

            public Vector3 upDirection => -Physics.gravity.normalized;
            public Vector3 movementDirection
            {
                get
                {
                    if (!canMove || m_walkVelocity == Vector3.zero)
                        return Vector3.zero;

                    return Vector3.ProjectOnPlane(m_walkVelocity, upDirection).normalized;
                }
            }

            public void Move(InputAction.CallbackContext context)
            {
                m_moveInput = Vector2.ClampMagnitude(context.ReadValue<Vector2>(), 1f);
            }

            /// <summary>
            /// Calculate how much the player should move this frame and apply it.
            /// </summary>
            private void MovePlayer()
            {
                // only do the boxcasts once per physics frame
                m_isGrounded = isGrounded;
                m_isHeaded = isHeaded;

                // project camera direction onto player direction for relative movement
                Vector3 camRight = Vector3.ProjectOnPlane(Camera.main.transform.right, model.transform.up);
                Vector3 camForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, model.transform.up);

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

                // if hitting head and moving and high enough speed, then bonk head
                if (m_isHeaded && m_verticalVelocity.magnitude > m_headBonkSpeed)
                    m_verticalVelocity = Vector3.zero;

                // only apply gravity if not grounded
                if (!m_isGrounded)
                    m_verticalVelocity += Physics.gravity * Time.fixedDeltaTime;

                if (!canMove && m_willJump)
                    m_willJump = false;

                // if the player should jump, add the jump velocity
                if (m_willJump)
                {
                    AddJumpForce();
                    m_willJump = false;
                    SFXController.Instance.PlayRandomSoundClip("Jump", transform);
                }

                // tell animator what to do
                animation.PassFloatParameter("Speed", m_walkVelocity.magnitude / m_topSpeed);

                // move player
                rb.velocity = m_walkVelocity + m_verticalVelocity + m_platformVelocity + m_externalVelocity;
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
                rb.velocity = Vector3.zero;
                m_verticalVelocity = Vector3.zero;
                m_externalVelocity = value;
            }

            [Header("Jumping")]
            [Tooltip("The velocity added to the player in units per second when they jump.")]
            [SerializeField] private float m_jumpSpeed;
            [Tooltip("Distance below bottom of player for grounded check.")]
            [SerializeField, Range(0, 0.5f)] private float m_groundCheckDistance;
            [Tooltip("The radius of the grounded check.")]
            [SerializeField, Range(0, 1)] private float m_groundCheckRadius;
            [Tooltip("The speed at which the player must be moving for them to be able to 'bonk' their head on ceilings.")]
            [SerializeField, Min(0)] private float m_headBonkSpeed;

            private bool m_isGrounded;
            private bool m_isHeaded;

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

            private void AddJumpForce()
            {
                m_verticalVelocity += m_jumpSpeed * upDirection;
            }

            /// Returns if the player is standing on a Walkable collider.
            /// </summary>
            public bool isGrounded
            {
                get
                {
                    if (!collider)
                        return false;

                    // check the space underneath the player to determine if grounded
                    // if there is no walkable object under the player, they are not grounded
                    RaycastHit[] hits = Physics.BoxCastAll(transform.position + collider.center,
                        new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        -upDirection, transform.rotation, collider.height / 2, m_walkableLayers, QueryTriggerInteraction.Ignore);

                    // if no hits, the player is not standing on anything
                    if (hits.Length == 0)
                    {
                        // reset value of surface normal
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
                            SFXController.Instance.PlayRandomSoundClip("Footsteps", transform);
                            m_verticalVelocity = Vector3.zero;
                            m_platformVelocity = Vector3.zero;
                            if (m_externalVelocity != Vector3.zero)
                            {
                                m_externalVelocity = Vector3.zero;
                                SetCanMove(true, CanMoveType.LevelObject);
                            }
                        }

                        // get closest hit walkable object
                        Vector3 playerFeet = transform.position + collider.center - upDirection * collider.height / 2;
                        RaycastHit hit = hits[0];
                        for (int h = 1; h < hits.Length; h++)
                        {
                            if (Vector3.Distance(hits[h].point, playerFeet) < Vector3.Distance(hit.point, playerFeet))
                                hit = hits[h];
                        }

                        // if the closest object does not meet the slope limit requirements, then the player is NOT standing on it
                        if (Vector3.Angle(hit.normal, upDirection) > m_slopeLimit)
                            return false;

                        // save the normal of the surface the player is standing on
                        m_surfaceNormal = hit.normal;

                        return true;
                    }

                    // ALTERNATE METHOD: checkbox
                    // return Physics.CheckBox(new Vector3(transform.position.x, transform.position.y - collider.height / 2, transform.position.z), 
                    //    new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius), Quaternion.identity, m_walkableLayers);
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
            }

            public bool isHeaded
            {
                get
                {
                    return Physics.BoxCast(transform.position + collider.center, new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        upDirection, /*out RaycastHit hit,*/ transform.rotation, collider.height / 2, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore);
                }
            }

#if UNITY_EDITOR
            [Header("Debug"), SerializeField] private bool m_drawGizmos;
            private void OnDrawGizmos()
            {
                if (!m_drawGizmos)
                    return;

                PlayerModel model;

                if (!GameManager.PlayerModel)
                {
                    model = GetComponentInChildren<PlayerModel>();
                    model.InitialiseCollider();
                }
                else
                    model = GameManager.PlayerModel;

                Handles.matrix = model.transform.localToWorldMatrix;

                Handles.color = Color.green;
                Handles.DrawWireCube(model.collider.center - Vector3.up * model.collider.height / 2,
                    new Vector3(m_groundCheckRadius * 2, m_groundCheckDistance * 2, m_groundCheckRadius * 2));

                Handles.color = Color.cyan;
                Handles.DrawWireCube(model.collider.center + Vector3.up * model.collider.height / 2,
                    new Vector3(m_groundCheckRadius * 2, m_groundCheckDistance * 2, m_groundCheckRadius * 2));
            }
#endif
        }
    }
}
