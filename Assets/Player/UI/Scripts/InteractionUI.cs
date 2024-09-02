///<summary>
/// Author: Halen
///
/// UI display for the Player Interaction system.
///
///</summary>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	namespace Player
	{
		using LevelObjects;

        namespace UI
		{
			public class InteractionUI : MonoBehaviour
			{
				[SerializeField] private RectTransform m_container;
				[SerializeField] private TextMeshProUGUI m_itemNameDisplay;

				[Header("Container Size Details"), Tooltip("Width of the container per character in an object's name.")]
				[SerializeField] private float m_widthPerChar = 17;
				[Tooltip("Constant height of the container.")]
                [SerializeField] private float m_height = 33;

				private LevelObject m_target;

                private void Start()
                {
                    SetDisplay(false);
                }

                private void Update()
                {
                    if (m_target)
					{
						m_container.position = Camera.main.WorldToScreenPoint(m_target.transform.position);
					}
                }

                public void UpdateDisplay(bool value, LevelObject target)
				{
					m_target = target;
					if (value)
					{
						m_itemNameDisplay.text = target.name;
						m_container.sizeDelta = new Vector2(m_widthPerChar * m_itemNameDisplay.text.Length, m_height);
					}

					SetDisplay(value);
				}

				private void SetDisplay(bool value)
				{
                    m_container.gameObject.SetActive(value);
                    m_itemNameDisplay.gameObject.SetActive(value);
                }
			}
		}
	}
}
