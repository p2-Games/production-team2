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
            private CharacterController m_characterController;
            private Camera m_camera;

            [Header("Player Details")]
            [SerializeField] private float m_moveSpeed;
            [SerializeField] private float m_lookSpeed;

            // input system controls
            private Vector2 m_moveDirection;
            private Vector2 m_lookDirection;
            
            void Start()
            {
                m_characterController = GetComponent<CharacterController>();
                m_camera = GetComponentInChildren<Camera>();
            }

            private void FixedUpdate()
            {
                MovePlayer();
                LookPlayer();
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
                //m_characterController.
            }
        }
    }
}
