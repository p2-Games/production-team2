///<summary>
/// Author: Emily
///
/// The reverse of the death animation
///
///</summary>

using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

namespace Millivolt
{
	namespace Player
	{
		namespace UI
		{
			public class PlayerSpawnUI : MonoBehaviour
			{
                [SerializeField] private Image m_growScreen;
                [SerializeField] private Image m_bgScreen;

                [SerializeField] private float m_startDelay;
                private float m_delay;

                private void Awake()
                {

                    Vector2 screenStartSize = m_growScreen.rectTransform.sizeDelta;
                    Vector2 screenEndSize = new Vector2(m_growScreen.rectTransform.sizeDelta.x, 5.0f);

                    Vector2 finalShrinksize = new Vector2(0, screenEndSize.y);

                    Tween.Size(m_growScreen.rectTransform, finalShrinksize, screenEndSize, 0.15f, m_startDelay, Tween.EaseInOutStrong);
                    m_delay += 0.17f;

                    Tween.Size(m_growScreen.rectTransform, m_growScreen.rectTransform.sizeDelta, new Vector2(800, 450), 0.2f, m_startDelay + m_delay, Tween.EaseInOutStrong);

                    m_delay += 0.2f;

                    Tween.Color(m_bgScreen, new Color(1, 1, 1, 0), 0.01f, m_startDelay + m_delay);
                    Tween.Color(m_growScreen, new Color(1, 1, 1, 0), 0.4f, m_startDelay + m_delay);

                    Destroy(gameObject, m_startDelay + 0.7f);
                }
            }
		}
	}
}