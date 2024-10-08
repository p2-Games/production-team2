///<summary>
/// Author: Halen
///
/// Allows the player to teleport to checkpoints as they unlock them.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace Level
    {
        public class CheckpointTeleporter : MonoBehaviour
        {
            [SerializeField] private Transform m_buttonTransform;
            [SerializeField] private bool m_checkpointsStartUnlocked = false;
            private bool[] m_unlockedCheckpoints;

            public void Setup()
            {
                int checkpointCount = LevelManager.Instance.checkpointCount;
                m_unlockedCheckpoints = new bool[checkpointCount];

                // unlock all the checkpoints
                if (m_checkpointsStartUnlocked)
                {
                    for (int a = 0; a < checkpointCount; a++)
                        m_unlockedCheckpoints[a] = true;
                }
                else
                {
                    // disable all the buttons until the player enters the related checkpoint
                    foreach (Transform button in m_buttonTransform)
                        button.gameObject.SetActive(false);
                }
            }

            public void UnlockCheckpoint(int index)
            {
                m_unlockedCheckpoints[index] = true;
            }
            
            public void TeleportToCheckpoint(int index)
            {
                LevelManager.Instance.GetCheckpoint(index).SetActiveCheckpoint();
                GameManager.PlayerStatus.Die();
            }
        }
    }
}
