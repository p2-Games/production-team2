///<summary>
/// Author: Emily
///
///	Displays the names for the credits in order
///
///</summary>

using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt.UI
{
	public class CreditsNamePlayer : MonoBehaviour
	{
        [SerializeField] private bool m_industryMode;
        [Space]

        [SerializeField] private float m_initialDelay;
		public List<CreditsName> m_peopleCredits;

		[SerializeField] private GameObject m_creditsPrefab;

        [Header("Credits Details")]
        [SerializeField] private float m_creditNameFadeInTime;
        [SerializeField] private float m_creditNameFadeOutTime;

        [Header("Menu Button")]
        [SerializeField] private Button m_menuButton;

        [Header("Spotlight")]
        [SerializeField] private GameObject m_spotLight;
        [SerializeField] private float m_spotlightActivationTime;

        [Header("CreditsWall Details")]
        [SerializeField] private CanvasGroup m_creditsWallCG;
        [SerializeField] private float m_creditsWallFadeTime;

        private bool m_playingCredits;

        private void Start()
        {
            Invoke(nameof(ActivateSpotLight), m_spotlightActivationTime);
            m_playingCredits = true;
            foreach (CreditsName person in m_peopleCredits)
            {
                StartCoroutine(PlayCreditsText(person));
            }
        }

        private IEnumerator PlayCreditsText(CreditsName person)
        {
            yield return new WaitForSeconds(person.startDelay + m_initialDelay);

            // Create credit prefab
            GameObject credit = Instantiate(m_creditsPrefab, gameObject.transform);
            credit.transform.position = person.spawnPosition.position;
            CreditsPrefabReferences creditsRef = credit.GetComponent<CreditsPrefabReferences>();

            // Fade in Credit
            Tween.CanvasGroupAlpha(creditsRef.GetComponent<CanvasGroup>(), 1, m_creditNameFadeInTime, 0);
            creditsRef.nameText.text = person.nameText;
            creditsRef.jobText.text = person.jobTitleText;

            // If there is no bg image then disable the image component on the prefab
            if (person.bgImage != null)
                creditsRef.bgImage.sprite = person.bgImage;
            else
                creditsRef.bgImage.enabled = false;

            if (m_industryMode)
                creditsRef.quoteText.text = "";
            else
                creditsRef.quoteText.text = person.quoteText;

            yield return new WaitForSeconds(person.lifetimeDuration);

            //If last name on the credits then activate the menu button
            if (person.nameText.Contains(m_peopleCredits[m_peopleCredits.Count - 1].nameText))
            {
                m_menuButton.gameObject.SetActive(true);
                Tween.CanvasGroupAlpha(m_menuButton.GetComponent<CanvasGroup>(), 1, 1, 0, null, Tween.LoopType.None, null, () => { SelectButton(); });
                Tween.CanvasGroupAlpha(m_creditsWallCG, 1, m_creditsWallFadeTime, 1);
                m_playingCredits = false;
            }

            //Fade out credit then destroy after finish
            Tween.CanvasGroupAlpha(creditsRef.GetComponent<CanvasGroup>(), 0, m_creditNameFadeOutTime, 0, null, Tween.LoopType.None, null, () => { DestroyCredit(credit); });
            yield return null;
        }

        private void DestroyCredit(GameObject credit)
        {
            Destroy(credit);
        }

        private void SelectButton()
        {
            m_menuButton.Select();
        }

        public void QuitToMenu()
        {
            GameManager.Instance.ExitToMenu();
        }

        private void ActivateSpotLight()
        {
            m_spotLight.SetActive(true);
        }

        public void SkipCredits()
        {
            StopAllCoroutines();
            var remainingCredits = FindObjectsByType<CreditsPrefabReferences>(FindObjectsSortMode.None);
            foreach (var credit in remainingCredits) 
                Destroy(credit.gameObject);
            m_menuButton.gameObject.SetActive(true);
            Tween.CanvasGroupAlpha(m_menuButton.GetComponent<CanvasGroup>(), 1, 1, 0, null, Tween.LoopType.None, null, () => { SelectButton(); });
            Tween.CanvasGroupAlpha(m_creditsWallCG, 1, m_creditsWallFadeTime, 0);
            m_playingCredits = false;
        }
    }
}
