///<summary>
/// Author: Emily
///
/// Component to add to object you want to delete itself after a delay
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	public class DestroySelfAfterDelay : MonoBehaviour
	{
		[SerializeField] private float m_destroyDelay;

        private void Start()
        {
			Destroy(gameObject, m_destroyDelay);
        }
    }
}
