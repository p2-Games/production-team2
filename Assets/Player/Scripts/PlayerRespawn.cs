///<summary>
/// Author: Halen
///
/// Handles the animation of respawning the player.
///
///</summary>

using Cinemachine;
using Millivolt.Level;
using Millivolt.Player.UI;
using UnityEngine;

namespace Millivolt.Player
{
    public class PlayerRespawn : MonoBehaviour
    {
        public static PlayerRespawn Instance;
        private void Awake()
        {
            if (Instance)
                Destroy(Instance.gameObject);
            Instance = this;
        }

        [SerializeField] private PlayerSpawnUI m_spawnScreenEffect;

        [SerializeField] private GameObject m_playerModel;
        [SerializeField] private ParticleSystem m_respawnParticle;
        [SerializeField] private bool m_drawGizmos = true;

        [Header("Timing")]
        [SerializeField, Min(0)] private float m_reactivatePlayerDelay;
        [SerializeField, Min(0)] private float m_resetCameraDelay;
        [SerializeField, Min(0)] private float m_giveControlBackDelay;

        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera m_vcam;
        [SerializeField] private float m_lookOffset = 1f;
        [SerializeField, Range(30, 120)] private float m_fieldOfView;
        [SerializeField] private Transform[] m_camAngles = new Transform[0];

        public void StartRespawn(Vector3 spawnPosition)
        {
            transform.position = m_playerModel.transform.position;

            m_spawnScreenEffect.gameObject.SetActive(true);

            // will change out for dissolve/anim
            m_playerModel.SetActive(false);
            m_vcam.Priority = 12;

            Transform chosenAngle = m_camAngles[Random.Range(0, m_camAngles.Length)];
            m_vcam.transform.SetPositionAndRotation(chosenAngle.position, chosenAngle.rotation);
            Instantiate(m_respawnParticle, m_playerModel.transform.position, m_respawnParticle.transform.rotation);

            Invoke(nameof(ReactivatePlayer), m_reactivatePlayerDelay);
        }

        private void ReactivatePlayer()
        {
            m_playerModel.SetActive(true);
            Invoke(nameof(ResetCamera), m_resetCameraDelay);
        }
        private void ResetCamera()
        {
            m_vcam.Priority = 0;
            Invoke(nameof(GiveControlBack), m_giveControlBackDelay);
        }

        private void GiveControlBack()
        {
            GameManager.PlayerController.SetCanMove(true, CanMoveType.Dead);
        }

        private void OnValidate()
        {
            if (m_vcam && m_fieldOfView != m_vcam.m_Lens.FieldOfView)
                m_vcam.m_Lens.FieldOfView = m_fieldOfView;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            int angles = transform.childCount;

            if (angles == 0)
                return;

            if (angles != m_camAngles.Length)
            {
                m_camAngles = new Transform[angles];
                for (int a = 0; a < angles; a++)
                    m_camAngles[a] = transform.GetChild(a);
            }

            Vector3 upDirection = -Physics.gravity.normalized;
            Gizmos.color = Color.cyan;

            foreach (Transform t in m_camAngles)
            {
                if (!t.gameObject.activeSelf)
                    continue;

                t.LookAt(m_playerModel.transform.position + upDirection * m_lookOffset, upDirection);

                if (m_drawGizmos)
                {
                    Gizmos.matrix = t.localToWorldMatrix;
                    Gizmos.DrawFrustum(Vector3.zero, m_fieldOfView, 3f, Camera.main.nearClipPlane, Camera.main.aspect);
                }
            }
        }
#endif
    }
}