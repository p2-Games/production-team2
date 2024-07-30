///<summary>
/// Author: Emily McDonald
///
/// This Handles the canvas for the healthbar fill
///
///</summary>

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	namespace UI
	{
		public class PlayerHealthBarUI : MonoBehaviour
		{
            [Header("Health Bar Propetries")]
			[SerializeField] private Image m_healthBarFill;
            [SerializeField] private Image m_healthBarColour;


            private void Start()
            {
                m_healthBarColour.color = new Color(0.6f, 0.85f, 1, 1);
            }

            /// <summary>
            /// Call this function to update the health bar
            /// </summary>
            /// <param name="currentHealth"></param>
            /// <param name="maxHealth"></param>
            public void UpdateHealthBar(float currentHealth, float maxHealth)
			{
				m_healthBarFill.fillAmount = (currentHealth / maxHealth);

                if ((currentHealth / maxHealth) <= 0.4f)
                {
                    m_healthBarColour.color = Color.Lerp(m_healthBarColour.color, new Color(1, 0, 0, 1), 0.2f);
                }
                else
                {
                    m_healthBarColour.color = Color.Lerp(m_healthBarColour.color, new Color(0.6f, 0.85f, 1, 1), 0.2f);
                }
            }
		}
	}
}
