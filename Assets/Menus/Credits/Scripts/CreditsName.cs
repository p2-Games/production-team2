///<summary>
/// Author: Emily
///
/// Holds all the references for the Credits objects
///
///</summary>

using TMPro;
using UnityEngine;

namespace Millivolt.UI
{
	[System.Serializable]
	public class CreditsName : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_nameRef;
		[SerializeField] private TextMeshProUGUI m_jobTitleRef;
		[SerializeField] private TextMeshProUGUI m_quoteRef;

		public string m_nameText;
		public string m_jobTitleText;
		public string m_quoteText;
	}
}
