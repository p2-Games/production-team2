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
        using UnityEngine.InputSystem;

        namespace UI
		{
			public class InteractionUI : MonoBehaviour
			{
				[SerializeField] private RectTransform m_container;
				[SerializeField] private TextMeshProUGUI m_itemNameDisplay;
				[SerializeField] private Image m_buttonDisplay;

				[Header("Container Size Details"), Tooltip("Width of the container per character in an object's name.")]
				[SerializeField] private float m_widthPerChar = 17;
				[Tooltip("Constant height of the container.")]
                [SerializeField] private float m_height = 33;

				private LevelObject m_target;

                private void Start()
                {
					m_buttonDisplay.rectTransform.sizeDelta = new Vector2(m_height, m_height);
					SetDisplayActive(false);
                }

                private void Update()
                {
                    if (m_target)
					{
						m_container.position = Camera.main.WorldToScreenPoint(m_target.transform.position);
					}
                }

                public void UpdateDisplay(LevelObject target)
				{
					m_target = target;
					if (m_target)
					{
						m_itemNameDisplay.text = target.name;
						m_container.sizeDelta = new Vector2(m_widthPerChar * m_itemNameDisplay.text.Length, m_height);
						m_buttonDisplay.sprite = PlayerInputIcons.Instance.GetInputIcon(InputType.Interact);
					}

					SetDisplayActive(m_target);
				}

				private void SetDisplayActive(bool value)
				{
                    m_container.gameObject.SetActive(value);
                    m_itemNameDisplay.gameObject.SetActive(value);
					m_buttonDisplay.gameObject.SetActive(value);
                }
			}
		}
	}
}
