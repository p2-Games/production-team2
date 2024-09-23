///<summary>
/// Author: Halen
///
/// Logic for controlling the Cinemachine cameras.
///
///</summary>

using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        public class CameraController : MonoBehaviour
        {
            [SerializeField] private CinemachineVirtualCamera m_normalCam;
            [SerializeField] private CinemachineVirtualCamera m_altCam;

            private CinemachineFramingTransposer m_normalBody;
            private CinemachineFramingTransposer m_altBody;

            [Header("Dynamic Movement"), SerializeField] private float m_screenXRange = 0.3f;
            [SerializeField] private float m_shoulderMoveSpeed = 3f;
            private float m_screenXVelocity;

            private float m_horizontalInput;

            private void Start()
            {
                m_normalBody = m_normalCam.GetCinemachineComponent<CinemachineFramingTransposer>();
                m_altBody = m_altCam.GetCinemachineComponent<CinemachineFramingTransposer>();
            }

            public void AltLook(InputAction.CallbackContext context)
            {
                if (context.performed)
                    m_altCam.Priority += 2;
                else if (context.canceled)
                    m_altCam.Priority -= 2;
            }

            public void Move(InputAction.CallbackContext context)
            {
                m_horizontalInput = context.ReadValue<Vector2>().x;
            }

            public void Update()
            {
                // calc target position of focus point
                float targetScreenX = 0.5f - m_horizontalInput * m_screenXRange;

                //float currentScreenX = Mathf.MoveTowards(m_normalBody.m_ScreenX, targetScreenX, m_dynamicAimSpeed);
                float currentScreenX = Mathf.SmoothDamp(m_normalBody.m_ScreenX, targetScreenX, ref m_screenXVelocity, m_shoulderMoveSpeed * Time.deltaTime);
                m_normalBody.m_ScreenX = currentScreenX;
                //m_altBody.m_ScreenX = currentScreenX;
            }
        }
    }
}
