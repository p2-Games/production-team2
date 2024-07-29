///<summary>
/// Author: Emily McDonald
///
/// This will change the opacity of a UI image of a vignette to add a hurt effect
///
///</summary>

using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	namespace UI
	{
		public class PlayerHurtEffect : MonoBehaviour
		{
			private Image m_vignette;

            private void Start()
            {
				m_vignette = GetComponentInChildren<Image>();
            }

            public void ChangeVignetteAlpha(float currentHealth, float maxHealth)
			{
				if ((currentHealth/maxHealth) <= 0.4f)
				{
					float alphaAmount = (1 - (currentHealth / maxHealth));
					m_vignette.color = new Color(m_vignette.color.r, m_vignette.color.g, m_vignette.color.b, Mathf.Lerp(m_vignette.color.a, alphaAmount, 0.1f));
				}
				else
				{
                    m_vignette.color = new Color(m_vignette.color.r, m_vignette.color.g, m_vignette.color.b, Mathf.Lerp(m_vignette.color.a, 0, 0.1f));
                }
			}
        }
	}
}
