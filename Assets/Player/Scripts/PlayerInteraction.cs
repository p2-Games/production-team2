///<summary>
/// Author: Halen
///
///
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	public class PlayerInteraction : MonoBehaviour
	{
		[SerializeField] private Collider m_trigger;

        private void Start()
        {
			m_trigger.isTrigger = true;
			m_trigger.enabled = false;
        }

        public void Interact()
		{
			m_trigger.enabled = true;
		}

        private void InteractWithInteractableObject(GameObject obj)
        {
            Debug.Log(obj.name + " was interacted with.");
            m_trigger.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interactable"))
                InteractWithInteractableObject(other.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Interactable"))
                InteractWithInteractableObject(collision.gameObject);
        }
    }
}
