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
            [SerializeField] private float m_axisBoundary = -20f;
            private bool m_highCamIsEnabled = false;

            [Header("Dynamic Shoulder"), SerializeField] private float m_screenXRange = 0.3f;
            [SerializeField] private float m_shoulderMoveSpeed = 3f;
            private float m_horizontalMoveDelta;
            private float m_screenXVelocity;

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
                m_horizontalMoveDelta = context.ReadValue<Vector2>().x;
            }

            public void Update()
            {
                // calc target position of focus point
                float targetScreenX = 0.5f - m_horizontalMoveDelta * m_screenXRange;

                //float currentScreenX = Mathf.MoveTowards(m_normalBody.m_ScreenX, targetScreenX, m_dynamicAimSpeed);
                float currentScreenX = Mathf.SmoothDamp(m_normalBody.m_ScreenX, targetScreenX, ref m_screenXVelocity, m_shoulderMoveSpeed * Time.deltaTime);
                m_normalBody.m_ScreenX = currentScreenX;
                //m_highBody.m_ScreenX = currentScreenX;

                // determine which camera to use for the better high look
                float normalValue = m_normalPOV.m_VerticalAxis.Value;
                float highValue = m_highPOV.m_VerticalAxis.Value;

                if (!m_highCamIsEnabled && normalValue < m_axisBoundary)
                {
                    m_highCam.Priority = 11;
                    m_highCamIsEnabled = true;
                }
                else if (m_highCamIsEnabled && highValue > m_axisBoundary)
                {
                    m_highCam.Priority = 9;
                    m_highCamIsEnabled = false;
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
