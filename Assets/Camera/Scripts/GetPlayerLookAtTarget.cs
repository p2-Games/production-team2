///<summary>
/// Author:
///
///
///
///</summary>

using Cinemachine;
using UnityEngine;

namespace Millivolt
{
	public class GetPlayerLookAtTarget : MonoBehaviour
	{
        private CinemachineVirtualCamera m_vcam;

        private void Awake()
        {
            m_vcam = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            if (m_vcam.LookAt = null)
            {
                if (GameManager.Player && GameManager.Player.Model)
                {
                    m_vcam.LookAt = GameManager.Player.Model.transform;
                }
            }
        }
    }
}
