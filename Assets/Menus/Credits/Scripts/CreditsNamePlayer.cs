///<summary>
/// Author: Emily
///
///	Displays the names for the credits in order
///
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Millivolt.UI
{
	public class CreditsNamePlayer : MonoBehaviour
	{
        [SerializeField] private bool m_industryMode;
        [Space]

        [SerializeField] private float m_initialDelay;
		public List<CreditsName> m_peopleCredits;

		[SerializeField] private GameObject m_creditsPrefab;

        private void Start()
        {
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

            Destroy(credit);
            yield return null;
        }
    }
}
