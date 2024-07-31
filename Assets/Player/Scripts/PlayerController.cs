///<summary>
/// Author: Halen Finlay
///
/// Handles the player movement, jumping, and look logic.
/// 
///</summary>

using Cinemachine;
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
                cursorIsLocked = true;
            }

            private void Update()
            {
                // set rotation of player object to face camera
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
            }

            private void FixedUpdate()
            {
                MovePlayer();
            }

            [Header("Physics")]
            [Tooltip("The acceleration of gravity of the player.")]
            [SerializeField] private Vector3 m_gravity;
            [Tooltip("The layers of objects that the CharacterController can interact with.")]
            [SerializeField] private LayerMask m_walkableLayers;

            private Rigidbody m_rb;
            private CapsuleCollider m_collider;

            [ContextMenu("Initialise Rigidbody")]
            private void InitialiseRigidbody()
            {
                m_rb = GetComponent<Rigidbody>();
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

            private Vector2 m_moveDirection;
            private Vector3 m_walkVelocity;
            private Vector3 m_verticalVelocity;
            private Vector3 m_externalVelocity;

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
                m_groundedLastFrame = isGrounded;

                // get camera directions
                Vector3 camRight = Camera.main.transform.right;
                Vector3 camForward = Camera.main.transform.forward;

                // 'flatten'
                camRight.y = 0;
                camForward.y = 0;

                // re-normalise now value has been edited
                camRight = camRight.normalized;
                camForward = camForward.normalized;

                // apply direction and speed
                Vector3 horizontalRelativeInput = m_moveDirection.x * camRight;
                Vector3 verticalRelativeInput = m_moveDirection.y * camForward;

                // calc movement vector
                Vector3 targetVelocity = (horizontalRelativeInput + verticalRelativeInput) * m_topSpeed;

                // calculate velocity change vector
                m_walkVelocity = Vector3.MoveTowards(m_walkVelocity, targetVelocity, m_moveDirection == Vector2.zero ? m_decceleration : m_acceleration);
                
                // only apply gravity if not grounded
                if (m_groundedLastFrame)
                    m_verticalVelocity = Vector3.zero;
                else 
                    m_verticalVelocity += m_gravity * Time.deltaTime;

                // if the player should jump, add the jump velocity
                if (m_willJump)
                {
                    m_verticalVelocity += transform.up * m_jumpSpeed;
                    m_willJump = false;
                }

                // move player
                m_rb.velocity = m_walkVelocity + m_verticalVelocity + m_externalVelocity;
            }

            [Header("Jumping")]
            [Tooltip("The velocity added to the player in units per second when they jump.")]
            [SerializeField] private float m_jumpSpeed;
            [Tooltip("Distance below bottom of player for grounded check.")]
            [SerializeField, Range(0,0.5f)] private float m_groundCheckDistance;
            [Tooltip("The radius of the grounded check.")]
            [SerializeField, Range(0,1)] private float m_groundCheckRadius;

            private bool m_groundedLastFrame;
            private bool m_willJump = false;

            /// <summary>
            /// If the player is standing on a Walkable collider.
            /// </summary>
            public bool isGrounded
            {
                get
                {
                    // check the space underneath the player
                    bool value = Physics.BoxCast(transform.position + m_collider.center, new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius),
                        -transform.up, out RaycastHit hit, Quaternion.identity, m_collider.height / 2, m_walkableLayers);

                    // if the hit object has a rigidbody, apply its velocity to the player.
                    if (hit.rigidbody)
                        m_externalVelocity = hit.rigidbody.GetPointVelocity(hit.point);
                    // otherwise reduce the external velocity based on decceleration
                    else
                    {
                        m_externalVelocity = Vector3.zero;
                        // float currentLength = m_externalVelocity.magnitude;
                        // m_externalVelocity.Normalize();
                        // m_externalVelocity *= currentLength / m_decceleration;
                    }

                    return value;

                    // checkbox (working)
                    //return Physics.CheckBox(new Vector3(transform.position.x, transform.position.y - m_collider.height / 2, transform.position.z), 
                    //   new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius), Quaternion.identity, m_walkableLayers);
                }
            }

            /// <summary>
            /// Make the player jump next time MovePlayer is called.
            /// </summary>
            public void Jump(InputAction.CallbackContext context)
            {
                if (context.started && m_groundedLastFrame)
                {
                    m_willJump = true;
                }
            }

            [Header("Camera")]
            [SerializeField] private CinemachineVirtualCamera m_virtualCam;
            private bool m_cursorIsLocked = false;
            public bool cursorIsLocked
            {
                get { return m_cursorIsLocked; }
                set
                {
                    if (value != m_cursorIsLocked)
                    {
                        if (value)
                        {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;
                        }
                        else
                        {
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.Confined;
                        }
                        m_cursorIsLocked = value;
                    }
                }
            }

            /// <summary>
            /// Change the move-speed of the camera.
            /// </summary>
            /// <param name="hSpeed">Horizontal camera speed.</param>
            /// <param name="vSpeed">Vertical camera speed.</param>
            public void SetCameraSensitivity(float hSpeed, float vSpeed)
            {
                var pov = m_virtualCam.GetCinemachineComponent<CinemachinePOV>();
                pov.m_HorizontalAxis.m_MaxSpeed = hSpeed;
                pov.m_VerticalAxis.m_MaxSpeed = vSpeed;
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
                    Handles.DrawWireCube(transform.position + m_collider.center + -transform.up * m_collider.height / 2,
                        new Vector3(m_groundCheckRadius, m_groundCheckDistance, m_groundCheckRadius));
                }
            }
#endif
        }
    }
}
