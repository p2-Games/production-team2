///<summary>
/// Author: Halen
///
/// Custom look target for the Cinemachine Free Look cam.
///
///</summary>

using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine.Utility;
using UnityEditor;
using UnityEngine.Rendering;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerLookTarget : MonoBehaviour
        {
            private enum LookType
            {
                Forward,
                Movement,
                Look,
                StrictOffset
            }
            
            [SerializeField] private LookType m_lookType;

            private LookType lookType
            {
                get => m_lookType;
                set
                {
                    m_variableTarget = Vector3.zero;

                    m_lookType = value;
                }
            }

            [SerializeField] private float m_followDistance;
            [SerializeField] private float m_followSpeed;
            [SerializeField] private Vector3 m_followOffset;
            [SerializeField] private float m_dampingStrength;

            [Space, SerializeField] private bool m_drawGizmos;

            private Transform m_player;
            private Transform m_camera;
            private Vector3 m_targetPosition;
            private Vector3 m_variableTarget;

            private Vector2 m_mouseDelta;
            private Vector2 m_inputDirection;

            private Vector3 positionWithOffset => m_player.position + followOffset;
            public Vector3 followOffset {
                get => m_followOffset.x * m_player.right + m_followOffset.y * m_player.up + m_followOffset.z * m_player.forward;
                set => m_followOffset = value;
            }

            public void Look(InputAction.CallbackContext context) => m_mouseDelta = context.ReadValue<Vector2>();
            public void Move(InputAction.CallbackContext context) => m_inputDirection = context.ReadValue<Vector2>();

            private void Start()
            {
                m_player = GameObject.FindWithTag("Player").transform;
                m_camera = GameObject.FindWithTag("MainCamera").transform;
            }

            public void Update()
            {
                float t = Damper.Damp(1, m_dampingStrength, Time.deltaTime);
                transform.position = Vector3.Slerp(transform.position, m_targetPosition, t);
            }

            public void FixedUpdate()
            {
                switch (m_lookType)
                {
                    case LookType.Forward:
                        m_targetPosition = positionWithOffset + m_player.forward * m_followDistance;
                        break;

                    case LookType.Movement:
                        m_variableTarget += (m_inputDirection.x * Vector3.ProjectOnPlane(m_camera.right, m_player.up) +
                                             m_inputDirection.y * Vector3.ProjectOnPlane(m_camera.forward, m_player.up)).normalized * m_followSpeed;
                        m_variableTarget = Vector3.ClampMagnitude(m_variableTarget, m_followDistance);
                        m_targetPosition = positionWithOffset + m_variableTarget;
                        break;

                    case LookType.Look:
                        m_variableTarget += (m_mouseDelta.x * Vector3.ProjectOnPlane(m_camera.right, m_player.up) +
                                             m_mouseDelta.y * Vector3.ProjectOnPlane(m_camera.forward, m_player.up)).normalized * m_followSpeed;
                        m_variableTarget = Vector3.ClampMagnitude(m_variableTarget, m_followDistance);
                        m_targetPosition = positionWithOffset + m_variableTarget;
                        break;

                    case LookType.StrictOffset:
                        m_targetPosition = positionWithOffset;
                        break;
                }
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (!m_drawGizmos)
                    return;

                if (!m_player)
                    Start();

                if (!Application.isPlaying)
                {
                    FixedUpdate();
                }

                Handles.color = Color.white;
                Handles.DrawWireCube(m_targetPosition, Vector3.one / 3f);

                Handles.color = Color.blue;
                Handles.DrawWireCube(transform.position, Vector3.one / 3f);
            }
#endif
        }
    }
}
