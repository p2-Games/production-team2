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
		public List<CreditsName> m_peopleCredits;

		[SerializeField] private GameObject m_creditsPrefab;

        private void Start()
        {
            StartCoroutine(PlayCreditsText(m_peopleCredits[0]));
        }

        private IEnumerator PlayCreditsText(CreditsName person)
        {
            yield return new WaitForSeconds(3);

            GameObject credit = Instantiate(m_creditsPrefab, gameObject.transform);
            credit.transform.position = person.spawnPosition.position;
            CreditsPrefabReferences creditsRef = credit.GetComponent<CreditsPrefabReferences>();
            creditsRef.nameText.text = person.nameText;
            creditsRef.jobText.text = person.jobTitleText;
            creditsRef.quoteText.text = person.quoteText;

            yield return new WaitForSeconds(5);

            Destroy(credit);
            yield return null;
        }
    }
}
