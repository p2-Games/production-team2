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
using UnityEngine.SceneManagement;
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

        [Header("Menu Button")]
        [SerializeField] private Button m_menuButton;

        [Header("Spotlight")]
        [SerializeField] private GameObject m_spotLight;
        [SerializeField] private float m_spotlightActivationTime;

        private void Start()
        {
            Invoke(nameof(ActivateSpotLight), m_spotlightActivationTime);
            foreach (CreditsName person in m_peopleCredits)
            {
                StartCoroutine(PlayCreditsText(person));
            }
        }

        private IEnumerator PlayCreditsText(CreditsName person)
        {
            yield return new WaitForSeconds(person.startDelay + m_initialDelay);

            GameObject credit = Instantiate(m_creditsPrefab, gameObject.transform);
            credit.transform.position = person.spawnPosition.position;
            CreditsPrefabReferences creditsRef = credit.GetComponent<CreditsPrefabReferences>();
            creditsRef.nameText.text = person.nameText;
            creditsRef.jobText.text = person.jobTitleText;
            creditsRef.bgImage.sprite = person.bgImage;
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
            }

            Destroy(credit);
            yield return null;
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
    }
}
