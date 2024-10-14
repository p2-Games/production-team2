///<summary>
/// Author: Halen
///
/// Component to handle behaviour for how and when cutscenes are played.
///
///</summary>

using Cinemachine;
using System.Collections;
using UnityEngine;

namespace Millivolt
{
    namespace Cutscene
    {
        [RequireComponent(typeof(CinemachineVirtualCamera))]
        public class CutsceneCamera : MonoBehaviour
        {
            [SerializeField, Min(0)] private float m_duration;

            private CinemachineVirtualCamera m_vcam;

            private void Start()
            {
                m_vcam = GetComponent<CinemachineVirtualCamera>();
            }

            public void StartCutscene()
            {
                StartCoroutine(Play());
            }

            private IEnumerator Play()
            {
                GameManager.PlayerController.SetCanMove(false, Player.CanMoveType.Cutscene);

                m_vcam.Priority += 10;
                yield return new WaitForSecondsRealtime(m_duration);
                m_vcam.Priority -= 10;

                GameManager.PlayerController.SetCanMove(true, Player.CanMoveType.Cutscene);
            }
        }
    }
}
