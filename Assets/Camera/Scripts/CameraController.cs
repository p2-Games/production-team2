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
    namespace Cameras
    {
        public class CameraController : MonoBehaviour
        {
            [SerializeField] private CinemachineVirtualCamera m_normalCam;
            [SerializeField] private CinemachineVirtualCamera m_highCam;

            private CinemachineFramingTransposer m_normalBody;
            private CinemachineFramingTransposer m_highBody;

            private CinemachinePOV m_normalPOV;
            private CinemachinePOV m_highPOV;

            [Header("Sensitivity")]
            public float horizontalSensitivity;
            public float verticalSensitivity;

            [Header("Transitioner")]
            [SerializeField] private float m_transitionBuffer = 5f;
            private bool m_highCamIsActive = false;

            [Header("Dynamic Shoulder"), SerializeField] private float m_screenXRange = 0.3f;
            [SerializeField] private float m_shoulderMoveSpeed = 3f;
            private float m_screenXVelocity;
            private float m_horizontalInput;

            [ContextMenu("Init")]
            private void Start()
            {
                m_normalBody = m_normalCam.GetCinemachineComponent<CinemachineFramingTransposer>();
                m_highBody = m_highCam.GetCinemachineComponent<CinemachineFramingTransposer>();

                m_normalPOV = m_normalCam.GetCinemachineComponent<CinemachinePOV>();
                m_highPOV = m_highCam.GetCinemachineComponent<CinemachinePOV>();

                // init sensitivity
                horizontalSensitivity = m_normalPOV.m_HorizontalAxis.m_MaxSpeed;
                verticalSensitivity = m_normalPOV.m_HorizontalAxis.m_MaxSpeed;
                m_highPOV.m_HorizontalAxis.m_MaxSpeed = horizontalSensitivity;
                m_highPOV.m_VerticalAxis.m_MaxSpeed = verticalSensitivity;
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
                //m_highBody.m_ScreenX = currentScreenX;

                // determine which camera to use for the better high look
                if (!m_highCamIsActive && m_normalPOV.m_VerticalAxis.Value <= m_normalPOV.m_VerticalAxis.m_MinValue + m_transitionBuffer)
                {
                    m_highCam.Priority += 2;
                    m_highCamIsActive = true;
                }
                else if (m_highCamIsActive && m_highPOV.m_VerticalAxis.Value >= m_highPOV.m_VerticalAxis.m_MaxValue - m_transitionBuffer)
                {
                    m_highCam.Priority -= 2;
                    m_highCamIsActive = false;
                }
            }

            public void UpdateSensitivity(float h, float v)
            {
                m_normalPOV.m_HorizontalAxis.m_MaxSpeed = horizontalSensitivity;
                m_normalPOV.m_VerticalAxis.m_MaxSpeed = verticalSensitivity;

                m_highPOV.m_HorizontalAxis.m_MaxSpeed = horizontalSensitivity;
                m_highPOV.m_VerticalAxis.m_MaxSpeed = verticalSensitivity;
            }
        }
    }
}
