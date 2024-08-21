///<summary>
/// Author: Halen
///
/// Makes a GameObject smoothly move and rotate to match the position and rotation of another object.
///
///</summary>

using UnityEngine;
using Cinemachine.Utility;
using UnityEditor;

namespace Millivolt
{
    public class SmoothObjectTracking : MonoBehaviour
    {
        [SerializeField] private Transform m_target;

        [Header("Movement"), Tooltip("How fast the object moves twoards its target in units per second.")]
        [SerializeField] private float m_moveSpeed;
        [Tooltip("Affects how smoothly the object moves.")]
        [SerializeField] private float m_moveSmoothing;
        [Tooltip("If the object should use the Slerp method for movement instead of the Lerp method.")]
        [SerializeField] private bool m_slerpMovement;

        [Header("Rotation"), Tooltip("How fast the object rotates towards its target in degrees per second.")]
        [SerializeField] private float m_rotateSpeed;
        [Tooltip("Affects how smoothly the object rotates.")]
        [SerializeField] private float m_rotateSmoothing;
        [Tooltip("If the object should use the Slerp method for rotation instead of the Lerp method.")]
        [SerializeField] private bool m_slerpRotation;

        [Space, SerializeField] private bool m_drawGizmos;

        private Vector3 m_targetPosition;
        private Quaternion m_targetRotation;

        private void Update()
        {
            float tMove = Damper.Damp(1, m_moveSmoothing, Time.deltaTime);
            if (m_slerpMovement)
                transform.position = Vector3.Slerp(transform.position, m_targetPosition, tMove);
            else
                transform.position = Vector3.Lerp(transform.position, m_targetPosition, tMove);

            float tRotate = Damper.Damp(1, m_rotateSmoothing, Time.deltaTime);
            if (m_slerpRotation)
                transform.rotation = Quaternion.Slerp(transform.rotation, m_targetRotation, tRotate);
            else
                transform.rotation = Quaternion.Lerp(transform.rotation, m_targetRotation, tRotate);
        }

        private void FixedUpdate()
        {
            m_targetPosition = Vector3.MoveTowards(transform.position, m_target.position, m_moveSpeed * Time.fixedDeltaTime);
            m_targetRotation = Quaternion.RotateTowards(transform.rotation, m_target.rotation, m_rotateSpeed * Time.fixedDeltaTime);
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

            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, transform.up, 0.75f);
        }
#endif
    }
}
