///<summary>
/// Author: Halen
///
/// Logic for controlling the Cinemachine cameras.
///
///</summary>

using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Cameras
    {
        public class CameraController : MonoBehaviour
        {
            [SerializeField] private CinemachineVirtualCamera m_3rdPersonCam;

            private CinemachineFramingTransposer m_camBody;
            private CinemachinePOV m_camPOV;

            //[Header("Sensitivity")]
            //public float horizontalSensitivity;
            //public float verticalSensitivity;

            [Header("High Look Transition")]
            [SerializeField] private float m_axisBoundary = -20f;
            [SerializeField] private Transform m_normalTarget;
            [SerializeField] private Transform m_highTarget;

            private bool m_highCamIsEnabled = false;

            [Header("Dynamic Shoulder"), SerializeField] private float m_screenXRange = 0.3f;
            [SerializeField] private float m_shoulderMoveSpeed = 3f;
            private float m_horizontalMoveDelta;
            private float m_screenXVelocity;

            [ContextMenu("Init")]
            private void Start()
            {
                // get cam components
                m_camBody = m_3rdPersonCam.GetCinemachineComponent<CinemachineFramingTransposer>();
                m_camPOV = m_3rdPersonCam.GetCinemachineComponent<CinemachinePOV>();

                // init sensitivity                
                m_camPOV.m_HorizontalAxis.m_MaxSpeed = PlayerSettings.Instance.horizontalSensitivity;
                m_camPOV.m_VerticalAxis.m_MaxSpeed = PlayerSettings.Instance.verticalSensitivity;
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
                float currentScreenX = Mathf.SmoothDamp(m_camBody.m_ScreenX, targetScreenX, ref m_screenXVelocity, m_shoulderMoveSpeed * Time.deltaTime);
                m_camBody.m_ScreenX = currentScreenX;
                //m_highBody.m_ScreenX = currentScreenX;

                // determine which camera to use for the better high look
                float verticalAxisValue = m_camPOV.m_VerticalAxis.Value;

                if (!m_highCamIsEnabled && verticalAxisValue < m_axisBoundary)
                {
                    m_highCamIsEnabled = true;
                    m_3rdPersonCam.Follow = m_highTarget;
                }
                else if (m_highCamIsEnabled && verticalAxisValue > m_axisBoundary)
                {
                    m_highCamIsEnabled = false;
                    m_3rdPersonCam.Follow = m_normalTarget;
                }
            }

            public void ScaleWithTimeScale()
            {
                m_camPOV.m_HorizontalAxis.m_MaxSpeed = PlayerSettings.Instance.horizontalSensitivity * Time.timeScale;
                m_camPOV.m_VerticalAxis.m_MaxSpeed = PlayerSettings.Instance.verticalSensitivity * Time.timeScale;
            }

            public void UpdateSensitivity(float h, float v)
            {
                m_camPOV.m_HorizontalAxis.m_MaxSpeed = h;
                m_camPOV.m_VerticalAxis.m_MaxSpeed = v;
            }

            private void ChangeFOV(float min, float max)
            {
                StopAllCoroutines();
                StartCoroutine(LerpFov(min, max));
            }

            private IEnumerator LerpFov(float min, float max)
            {
                float t = 0;
                while (m_3rdPersonCam.m_Lens.FieldOfView != max)
                {
                    m_3rdPersonCam.m_Lens.FieldOfView = Mathf.Lerp(min, max, t);

                    t += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                m_3rdPersonCam.m_Lens.FieldOfView = max;
            }
        }
    }
}
