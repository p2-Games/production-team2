///<summary>
/// Author: Halen
///
/// Controls the parent transform of the First Person Cinemachine virtual camera.
///
///</summary>

using Cinemachine.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        public class FirstPersonCameraController : MonoBehaviour
        {
            [Header("Camera"), SerializeField] private Camera m_camera;
            [SerializeField] private Vector3 m_offset;
            private bool m_cursorIsLocked = false;

            [Header("Control Values")]
            [SerializeField] private float m_sensitivity;
            [SerializeField, Range(0, 90)] private float m_verticalLookClamp;
            [SerializeField, Range(0, 10)] private float m_damping;

            private Transform m_player;
            private Vector2 m_mouseDelta;
            private Vector2 m_targetRotation;

            public Vector2 targetRotation => m_targetRotation;

            private Quaternion m_targetPlayerRotation;
            private Quaternion m_targetCameraRotation;

            public void LookInput(InputAction.CallbackContext context)
            {
                m_mouseDelta = context.ReadValue<Vector2>();
                //Debug.Log(m_mouseDelta);
            }

            private void Start()
            {
                // lock the cursor
                cursorIsLocked = true;

                // get camera if not set in inspector
                if (!m_camera)
                    m_camera = Camera.main;

                // get player
                m_player = transform.GetChild(0);

                // get default rotation from inspector set values
                m_targetRotation.y = m_camera.transform.eulerAngles.x;
                m_targetRotation.x = m_player.transform.eulerAngles.y;

                // set offset
                m_camera.transform.localPosition = m_offset;
            }

            private void Update()
            {
                float tCamera = Damper.Damp(1, m_damping, Time.deltaTime);
                m_camera.transform.localRotation = Quaternion.Slerp(m_camera.transform.localRotation, m_targetCameraRotation, tCamera);

                float tPlayer = Damper.Damp(1, m_damping, Time.fixedDeltaTime);
                m_player.transform.localRotation = Quaternion.Slerp(m_player.transform.localRotation, m_targetPlayerRotation, tPlayer);
            }

            private void FixedUpdate()
            {
                // get input value
                Vector2 input = m_mouseDelta * m_sensitivity;

                // clamp the vertical rotation
                m_targetRotation.y -= input.y;
                m_targetRotation.y = Mathf.Clamp(m_targetRotation.y, -m_verticalLookClamp, m_verticalLookClamp);

                // rotate camera vertically with damping
                m_targetCameraRotation = Quaternion.Euler(m_targetRotation.y, 0, 0);
                
                // wrap the horizontal rotation
                m_targetRotation.x += input.x;
                if (m_targetRotation.x > 360f)
                    m_targetRotation.x -= 360f;
                else if (m_targetRotation.x < 0)
                    m_targetRotation.x += 360f;

                // rotate player horizontally with damping
                m_targetPlayerRotation = Quaternion.Euler(0, m_targetRotation.x, 0);
            }

            public void SetLookRotation(float x, float y)
            {
                m_targetRotation = new Vector2(x, y);
            }

            public void SetLookRotation(Vector2 value)
            {
                m_targetRotation = value;
            }

            public void SetLookRotation(Vector3 eulers)
            {
                m_targetRotation = new Vector2(eulers.y, eulers.x);
            }

            public void SetLookRotation(Quaternion value)
            {
                Vector3 eulers = value.eulerAngles;
                m_targetRotation = new Vector2(eulers.y, eulers.x);
            }

            /// <summary>
            /// If the mouse cursor is locked to the centre of the screen or not.
            /// </summary>
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
                            Cursor.lockState = CursorLockMode.None;
                        }
                        m_cursorIsLocked = value;
                    }
                }
            }

            /// <summary>
            /// Change the sensitivity of the camera.
            /// </summary>
            public void SetSensitivity(float value)
            {
                m_sensitivity = value;
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (!m_player)
                    return;

                Handles.color = Color.magenta;
                Handles.DrawLine(m_player.transform.position + transform.up * -1.5f, m_player.transform.position + transform.up * 1.5f);
            }
#endif
        }
    }
}
