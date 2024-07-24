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
        [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
        public class PlayerController : MonoBehaviour
        {
            void Start()
            {
                m_characterController = GetComponent<CharacterController>();
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
            [SerializeField] private float m_mass;
            [SerializeField] private Vector3 m_gravity;

            public void SetMass(float value)
            {
                m_mass = value;
            }

            public void SetGravity(Vector3 value)
            {
                m_gravity = value;
            }

            [Header("Movement")]
            [SerializeField] private float m_moveSpeed;
            private CharacterController m_characterController;
            private Vector2 m_moveDirection;

            public void Move(InputAction.CallbackContext context)
            {
                m_moveDirection = context.ReadValue<Vector2>();
            }

            /// <summary>
            /// Calculate how much the player should move this frame and apply it.
            /// </summary>
            private void MovePlayer()
            {
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
                Vector3 horizontalRelativeInput = m_moveDirection.x * camRight * m_moveSpeed;
                Vector3 verticalRelativeInput = m_moveDirection.y * camForward * m_moveSpeed;

                // only do the boxcast once per frame
                bool thisFrameIsGrounded = isGrounded;

                // if player just landed, reset their vertical velocity to 0
                if (!m_groundedLastFrame && thisFrameIsGrounded)
                    m_verticalVelocity = Vector3.zero;

                // update for next check
                m_groundedLastFrame = thisFrameIsGrounded;

                // if the player should jump, add the jump velocity
                if (m_willJump)
                {
                    m_verticalVelocity += transform.up * m_jumpSpeed;
                    m_willJump = false;
                }

                // apply gravity if player is NOT grounded
                if (!thisFrameIsGrounded)
                    m_verticalVelocity += m_gravity;

                // calculate movement vector
                Vector3 movement = horizontalRelativeInput + verticalRelativeInput + m_verticalVelocity;
                
                // move player
                //transform.Translate(movement * Time.deltaTime, Space.World);
                m_characterController.Move(movement * Time.deltaTime);
            }

            [Header("Jumping")]
            [SerializeField] private float m_jumpSpeed;
            [SerializeField, Range(0,0.5f)] private float m_groundCheckOffset;
            [SerializeField, Range(0,1)] private float m_groundCheckRadius;
            private bool m_groundedLastFrame;
            private Vector3 m_verticalVelocity = Vector3.zero;
            private bool m_willJump = false;

            /// <summary>
            /// If the player is standing on floor or another object.
            /// </summary>
            public bool isGrounded
            {
                get
                {
                    Vector3 playerFeet = new Vector3(transform.position.x, transform.position.y - m_characterController.height / 2, transform.position.z);
                    // check the space underneath the player
                    if (Physics.CheckBox(playerFeet, new Vector3(m_groundCheckRadius, m_groundCheckOffset, m_groundCheckRadius), Quaternion.identity))
                    {
                        return true;
                    }
                    return false;
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
            private void OnDrawGizmos()
            {
                Handles.color = Color.green;
                if (m_characterController)
                    Handles.DrawWireCube(new Vector3(transform.position.x, transform.position.y - m_characterController.height / 2, transform.position.z),
                        new Vector3(m_groundCheckRadius * 2, m_groundCheckOffset * 2, m_groundCheckRadius * 2));
            }
#endif
        }
    }
}
