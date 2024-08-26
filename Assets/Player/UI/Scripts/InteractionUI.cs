///<summary>
/// Author:
///
///
///
///</summary>

using TMPro;
using UnityEngine;

namespace Millivolt
{
	namespace Player
	{
		using Interaction;
        using UnityEngine.UI;

        namespace UI
		{
			public class InteractionUI : MonoBehaviour
			{
				[SerializeField] private Image m_background;
				[SerializeField] private TextMeshProUGUI m_itemNameDisplay;

				private PlayerInteraction m_player;

                private void Start()
                {
					m_player = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerInteraction>();
                    SetDisplay(false);
                }

                public void UpdateDisplay()
				{
					if (m_player.interactionState == InteractionState.Holding)
					{
						SetDisplay(true);
                        m_itemNameDisplay.text = m_player.heldItemName;
					}
					else
					{
						SetDisplay(false);
					}
				}

				private void SetDisplay(bool value)
				{
                    m_background.gameObject.SetActive(value);
                    m_itemNameDisplay.gameObject.SetActive(value);
                }
			}
		}
	}
}
