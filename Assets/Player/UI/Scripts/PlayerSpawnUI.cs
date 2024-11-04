///<summary>
/// Author: Halen
///
/// Plays when the player spawns.
///
///</summary>

using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt.Player.UI
{
    public class PlayerSpawnUI : MonoBehaviour
    {
        [SerializeField] private Image m_growScreen;
        [SerializeField] private Image m_bgScreen;

        [SerializeField] private float m_startDelay;
        private float m_delay;

        private float m_disableDelay;

        private void OnEnable()
        {
            Vector2 screenStartSize = m_growScreen.rectTransform.sizeDelta;
            Vector2 screenEndSize = new Vector2(m_growScreen.rectTransform.sizeDelta.x, 5.0f);

            Vector2 finalShrinksize = new Vector2(0, screenEndSize.y);

            Tween.Size(m_growScreen.rectTransform, finalShrinksize, screenEndSize, 0.15f, m_startDelay, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, false);
            m_delay += 0.17f;
            m_disableDelay += 0.17f;

            Tween.Size(m_growScreen.rectTransform, m_growScreen.rectTransform.sizeDelta, new Vector2(800, 450), 0.2f, m_startDelay + m_delay, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, false);

            m_delay += 0.2f;
            m_disableDelay += 0.2f;

            Tween.Color(m_bgScreen, new Color(0, 0, 0, 0), 0.01f, m_startDelay + m_delay, null, Tween.LoopType.None, null, null, false);
            Tween.Color(m_growScreen, new Color(m_growScreen.color.r, m_growScreen.color.g, m_growScreen.color.b, 0), 0.4f, m_startDelay + m_delay, null, Tween.LoopType.None, null, null, false);

            m_disableDelay += 0.5f;

            //Destroy(gameObject, m_startDelay + 0.7f);

            Invoke(nameof(DisableThis), m_startDelay + m_disableDelay);
        }
        protected virtual void DisableThis()
        {
            gameObject.SetActive(false);
            m_delay = 0;
            m_disableDelay = 0;
            Tween.Size(m_growScreen.rectTransform, m_growScreen.rectTransform.sizeDelta, new Vector2(0, 0), 0, 0 + m_delay, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, false);
            Tween.Color(m_bgScreen, new Color(0, 0, 0, 1), 0, 0, null, Tween.LoopType.None, null, null, false);
            Tween.Color(m_growScreen, new Color(1, 1, 1, 1), 0, 0);
        }
    }
}
