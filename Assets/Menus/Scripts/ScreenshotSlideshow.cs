///<summary>
/// Author: Emily
///
/// Handles swapping the image for the slideshow panel on the endscreen
///
///</summary>

using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt.UI
{
	public class ScreenshotSlideshow : MonoBehaviour
	{
		[SerializeField] private Image m_slideshow;
		[SerializeField] private Sprite[] m_screenshots;
		[SerializeField] private CanvasGroup m_screenshotCanvasGroup;
		private int m_screenshotIndex;

		[SerializeField] private CanvasGroup m_bgCanvasGroup;

		[Header("Fade to black details")]
		[SerializeField] private float m_bgFadeDuration;
		[SerializeField] private float m_bgFadeDelay;

		[Header("Slideshow Details")]
		[SerializeField] private float m_slideDuration;
		[SerializeField] private float m_slideFadeDuration;
		[SerializeField] private float m_slideFadeDelay;

        private void Start()
        {
			//Fade screen to black then call the Slide show loop
			Tween.CanvasGroupAlpha(m_bgCanvasGroup, 1, m_bgFadeDuration, m_bgFadeDelay, null, Tween.LoopType.None, null, () => { FadeOut(); }, false);
        }

		private void FadeOut()
		{
			//Set the canvas group alpha of the screenshot to 0, use callback to Fade back in after the tween is complete
			Tween.CanvasGroupAlpha(m_screenshotCanvasGroup, 0, m_slideFadeDuration, m_slideFadeDelay, null, Tween.LoopType.None, null, () => { FadeIn(); }, false);
		}

		private void FadeIn()
		{
			//Update the index so it shows a different image in the array
			m_screenshotIndex++;
			//If the index is beyond the range of the array then reset the index
			if (m_screenshotIndex > m_screenshots.Length - 1)
				m_screenshotIndex = 0;
			//Change the sprite on the image panel to be what the current image should be
			m_slideshow.sprite = m_screenshots[m_screenshotIndex];
			//Fade the screenshot in then call fade out again
            Tween.CanvasGroupAlpha(m_screenshotCanvasGroup, 1, m_slideFadeDuration, m_slideFadeDuration, null, Tween.LoopType.None, null, () => { FadeOut(); }, false);
        }
    }
}
