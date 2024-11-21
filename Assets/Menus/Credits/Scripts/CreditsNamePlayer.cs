///<summary>
/// Author: Emily
///
///	Displays the names for the credits in order
///
///</summary>

using System.Collections.Generic;
using UnityEngine;

namespace Millivolt.UI
{
	public class CreditsNamePlayer : MonoBehaviour
	{
		public List<CreditsName> m_peopleCredits;

		[SerializeField] private GameObject m_creditsPrefab;
	}
}
