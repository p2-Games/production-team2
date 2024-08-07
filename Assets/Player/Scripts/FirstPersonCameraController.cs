///<summary>
/// Author: Halen
///
/// Controls the parent transform of the First Person Cinemachine virtual camera.
///
///</summary>

using Cinemachine;
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
                //transform.position = m_player.position;
            }

            private void FixedUpdate()
            {
                // get input value
                Vector2 input = m_mouseDelta * m_sensitivity;

                // clamp the vertical rotation
                m_targetRotation.y -= input.y;
                m_targetRotation.y = Mathf.Clamp(m_targetRotation.y, -m_verticalLookClamp, m_verticalLookClamp);

                // rotate camera vertically with damping
                float tCamera = Damper.Damp(1, m_damping, Time.fixedDeltaTime);
                Quaternion newVerticalRotation = Quaternion.Euler(m_targetRotation.y, 0, 0);
                m_camera.transform.localRotation = Quaternion.Slerp(m_camera.transform.localRotation, newVerticalRotation, tCamera);

                /*
                float t = Damper.Damp(1, m_damping, Time.deltaTime);
                Quaternion newRotation = Quaternion.LookRotation(m_mouseDelta.normalized, m_virtualCam.transform.up);
                m_virtualCam.transform.rotation = Quaternion.Slerp(m_virtualCam.transform.rotation, newRotation, t);
                */

                // wrap the horizontal rotation
                m_targetRotation.x += input.x;
                if (m_targetRotation.x > 360f)
                    m_targetRotation.x -= 360f;
                else if (m_targetRotation.x < 0)
                    m_targetRotation.x += 360f;

                // rotate player horizontally with damping
                float tPlayer = Damper.Damp(1, m_damping, Time.fixedDeltaTime);
                Quaternion newHorizontalRotation = Quaternion.Euler(0, m_targetRotation.x, 0);
                m_player.transform.localRotation = Quaternion.Slerp(m_player.transform.localRotation, newHorizontalRotation, tPlayer);

                /*
                float tPlayer = Damper.Damp(1, 0.1f, Time.deltaTime);
                Vector3 forward = Vector3.Project(m_virtualCam.transform.forward, transform.forward);
                Quaternion newPlayerRotation = Quaternion.LookRotation(forward, transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, newPlayerRotation, tPlayer);
                */
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
            /// Change the move-speed of the camera.
            /// </summary>
            /// <param name="hSpeed">Horizontal camera speed.</param>
            /// <param name="vSpeed">Vertical camera speed.</param>
            public void SetCameraSensitivity(float value)
            {
                m_sensitivity = value;
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (!m_player)
                    return;

                Handles.color = Color.magenta;
                Handles.DrawLine(m_player.transform.position + m_player.transform.up * -1.5f, m_player.transform.position + m_player.transform.up * 1.5f);
            }
#endif
        }
    }
}
