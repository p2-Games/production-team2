///<summary>
/// Author: Halen
///
/// Allows the player to teleport to checkpoints as they unlock them.
///
///</summary>

using Millivolt.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Millivolt
{
    namespace Level
    {
        public class CheckpointTeleporter : MonoBehaviour
        {
            [SerializeField] private Transform m_buttonTransform;
            [SerializeField] private Button m_buttonPrefab;
            [SerializeField] private bool m_checkpointsStartUnlocked = false;
            private bool[] m_unlockedCheckpoints;

            [SerializeField] private UIMenu m_containerUI;

            private int m_selectedButton;

            public void Start()
            {
                int checkpointCount = transform.childCount;
                m_unlockedCheckpoints = new bool[checkpointCount];

                InitialiseButtons(checkpointCount);

                // unlock all the checkpoints
                if (m_checkpointsStartUnlocked)
                {
                    for (int a = 0; a < checkpointCount; a++)
                        m_unlockedCheckpoints[a] = true;
                }
                else
                {
                    // disable all the buttons until the player enters the related checkpoint
                    for (int b = 1; b < checkpointCount; b++)
                    {
                        GameObject button = m_buttonTransform.GetChild(b).gameObject;
                        button.SetActive(false);
                    }
                }

                m_containerUI.gameObject.SetActive(false);
                //m_buttonTransform.gameObject.SetActive(false);
            }

            public bool CheckpointIsUnlocked(int index)
            {
                return m_unlockedCheckpoints[index];
            }

            public void UnlockCheckpoint(int index)
            {
                m_unlockedCheckpoints[index] = true;
            }
            
            public void TeleportToCheckpoint(int index)
            {
                SetDisplayActive(false);
                LevelManager.Instance.GetCheckpoint(index).SetActiveCheckpoint();
                GameManager.Player.Status.Die();
            }

            public void InitialiseButtons(int checkpointCount)
            {
                for (int c = 0; c < checkpointCount; c++)
                {
                    Button button = Instantiate(m_buttonPrefab, m_buttonTransform);
                    AddButtonListener(button, c);
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Checkpoint " + (c + 1).ToString();
                }
            }

            public void SetActiveButton(int index)
            {
                m_selectedButton = index;
                for (int d = 0; d < m_buttonTransform.childCount; d++)
                {
                    if (d == m_selectedButton)
                        m_buttonTransform.GetChild(d).GetComponent<ButtonBehaviour>().ActivateButton();
                    else
                        m_buttonTransform.GetChild(d).GetComponent<ButtonBehaviour>().DeactivateButton();
                }
            }

            private void AddButtonListener(Button button, int param)
            {
                button.onClick.AddListener(() => TeleportToCheckpoint(param));

                EventTrigger trigger = GetComponentInParent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((eventData) => { SetActiveButton(param); });
                //trigger.delegates.Add(entry);
            }

            public void SetDisplayActive(bool value)
            {
                //if (m_buttonTransform.gameObject.activeSelf != value)
                //m_buttonTransform.gameObject.SetActive(value);

                if (value)
                {
                    m_containerUI.firstSelected = m_buttonTransform.transform.GetChild(0).gameObject;
                    m_containerUI.ActivateMenu();
                }
                else
                    m_containerUI.DeactivateMenu();
            }
        }
    }
}
