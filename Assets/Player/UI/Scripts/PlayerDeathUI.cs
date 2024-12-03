///<summary>
/// Author: Emily
///
/// Using tweens to replicate the effect of old school tv turning off
///
///</summary>

using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;
using Millivolt.Level;
using Millivolt.Sound;

namespace Millivolt
{
    namespace Player
    {
        namespace UI
        {
	        public class PlayerDeathUI : MonoBehaviour
	        {
		        [SerializeField] private Image m_shrinkScreen;

		        private float m_delay;

                private float m_disableDelay;

                private void OnEnable()
                {

                    Vector2 screenStartSize = m_shrinkScreen.rectTransform.sizeDelta;
                    Vector2 screenEndSize = new Vector2(m_shrinkScreen.rectTransform.sizeDelta.x, 5.0f);

                    Tween.Size(m_shrinkScreen.rectTransform, screenStartSize, screenEndSize, 0.15f, 0, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, false);

                    m_delay += 0.17f;
                    m_disableDelay += 0.17f;

                    Vector2 finalShrinksize = new Vector2(0, screenEndSize.y);

                    Tween.Size(m_shrinkScreen.rectTransform, m_shrinkScreen.rectTransform.sizeDelta, finalShrinksize, 0.2f, m_delay, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, false);

                    m_disableDelay += 1f;
                    //Destroy(gameObject, 1f);

                    // play a death sound effect
                    SFXController.Instance.PlayRandomSoundClip("PlayerDamage", transform.parent);

                    Invoke(nameof(DisableThis), m_disableDelay);
                }

                private void DisableThis()
                {
                    gameObject.SetActive(false);
                    m_delay = 0;
                    m_disableDelay = 0;
                    Tween.Size(m_shrinkScreen.rectTransform, m_shrinkScreen.rectTransform.sizeDelta, new Vector2(800, 450), 0, 0, Tween.EaseInOutStrong, Tween.LoopType.None, null, null, false);

                    // initiate respawn sequence
                    LevelManager.Instance.SpawnPlayer();

                    // play respawn effect
                    PlayerRespawn.Instance.StartRespawn(LevelManager.Instance.activeCheckpoint.transform.position + new Vector3(0, 0.6f, 0));
                }
            }
        }
    }
}
