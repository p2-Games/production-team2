///<summary>
/// Author: Emily
///
/// Holds all the references for the Credits objects
///
///</summary>

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Millivolt.UI
{
	[System.Serializable]
	public class CreditsName
	{
		public string nameText;
		public string jobTitleText;
		public string quoteText;

		public Sprite bgImage;

		public Transform spawnPosition;

		public float startDelay;
		public float lifetimeDuration;
	}
}
