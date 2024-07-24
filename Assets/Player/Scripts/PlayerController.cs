///<summary>
/// Author: Halen Finlay
///
/// Handles the player movement and look logic.
/// 
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerController : MonoBehaviour
        {
            // components
            private CharacterController m_characterController;

            [Header("Movement")]
            [SerializeField] private float m_moveSpeed;
            private Vector2 m_moveDirection;

            [Header("Camera")]
            [SerializeField] private Camera m_camera;
            [SerializeField] private float m_lookSensitivity;
            [SerializeField] private Vector2 m_cameraVerticalLook;
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
            private Vector2 m_lookDirection;
            
            void Start()
            {
                m_characterController = GetComponent<CharacterController>();
                cursorIsLocked = true;
            }

            private void Update()
            {
                LookPlayer();
            }

            private void FixedUpdate()
            {
                MovePlayer();
            }

            // Input System methods
            public void Move(InputAction.CallbackContext context)
            {
                m_moveDirection = context.ReadValue<Vector2>();
            }

            public void Look(InputAction.CallbackContext context)
            {
                m_lookDirection = context.ReadValue<Vector2>();
            }

            // private methods
            private void MovePlayer()
            {
                m_characterController.Move(new Vector3(m_moveDirection.x, 0, m_moveDirection.y) * m_moveSpeed * Time.deltaTime);
            }

            private void LookPlayer()
            {
                // keep value between 0 and 360
                if (m_lookDirection.x >= 360f)
                    m_lookDirection.x -= 360;
                else if (m_lookDirection.x < 0)
                    m_lookDirection.x += 360;

                // clamp vertical look
                m_lookDirection.y = Mathf.Clamp(m_lookDirection.y, m_cameraVerticalLook.x, m_cameraVerticalLook.y);

                // apply vertical rotation
                m_camera.transform.eulerAngles = Vector3.right * m_lookDirection.y;

                // apply rotation
                transform.Rotate(Vector3.up * m_lookDirection.x);
            }
        }
    }
}
