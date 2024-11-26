///<summary>
/// Author: Emily
///
/// Hold the references for the TMP fields on the credits name prefab
///
///</summary>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt
{
	public class CreditsPrefabReferences : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_nameText;
		[SerializeField] private TextMeshProUGUI m_jobText;
		[SerializeField] private TextMeshProUGUI m_quoteText;

		[SerializeField] private Image m_bgImage;

		public TextMeshProUGUI nameText => m_nameText;
		public TextMeshProUGUI jobText => m_jobText;
		public TextMeshProUGUI quoteText => m_quoteText;

		public Image bgImage => m_bgImage;
	}
}
