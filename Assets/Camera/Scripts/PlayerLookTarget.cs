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

            [Tooltip("The target Transform.\n Should be the Player's Motor or the Player Follower")]
            [SerializeField] private Transform m_target;

            [Tooltip("The max distance it can be from its target.")]
            [SerializeField] private float m_followDistance;
            [Tooltip("For Look Types that don't have a set target, the speed at which the object moves towards its target.")]
            [SerializeField] private float m_followSpeed;
            [Tooltip("A static offset value for the look target.")]
            [SerializeField] private Vector3 m_followOffset;
            [Tooltip("How smooth movement is.")]
            [SerializeField] private float m_dampingStrength;

            [Space, SerializeField] private bool m_drawGizmos;

            private Transform m_camera;
            private Vector3 m_targetPosition;
            private Vector3 m_variableTarget;

            private Vector2 m_mouseDelta;
            private Vector2 m_inputDirection;

            private Vector3 targetPosition => m_target.position + followOffset;
            public Vector3 followOffset {
                get => m_followOffset.x * m_target.right + m_followOffset.y * m_target.up + m_followOffset.z * m_target.forward;
                set => m_followOffset = value;
            }

            public void Look(InputAction.CallbackContext context) => m_mouseDelta = context.ReadValue<Vector2>();
            public void Move(InputAction.CallbackContext context) => m_inputDirection = context.ReadValue<Vector2>();

            private void Start()
            {
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
                        m_targetPosition = targetPosition + m_target.forward * m_followDistance;
                        break;

                    case LookType.Movement:
                        m_variableTarget += (m_inputDirection.x * Vector3.ProjectOnPlane(m_camera.right, m_target.up) +
                                             m_inputDirection.y * Vector3.ProjectOnPlane(m_camera.forward, m_target.up)).normalized * m_followSpeed;
                        m_variableTarget = Vector3.ClampMagnitude(m_variableTarget, m_followDistance);
                        m_targetPosition = targetPosition + m_variableTarget;
                        break;

                    case LookType.Look:
                        m_variableTarget += (m_mouseDelta.x * Vector3.ProjectOnPlane(m_camera.right, m_target.up) +
                                             m_mouseDelta.y * Vector3.ProjectOnPlane(m_camera.forward, m_target.up)).normalized * m_followSpeed;
                        m_variableTarget = Vector3.ClampMagnitude(m_variableTarget, m_followDistance);
                        m_targetPosition = targetPosition + m_variableTarget;
                        break;

                    case LookType.StrictOffset:
                        m_targetPosition = targetPosition;
                        break;
                }
            }

            [ContextMenu("Reset position")]
            public void SetToPlayerPosition()
            {
                transform.position = m_target.position;
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (!m_drawGizmos)
                    return;

                if (!m_camera)
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
