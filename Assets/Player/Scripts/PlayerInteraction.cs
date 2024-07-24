///<summary>
/// Author: Halen
///
/// Handles interactions between the player and objects that can be interacted with in levels.
///
///</summary>

using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerInteraction : MonoBehaviour
        {
            private Collider m_trigger;

            private void Start()
            {
                m_trigger = GetComponent<SphereCollider>();
                m_trigger.isTrigger = true;
                m_trigger.enabled = false;
            }

            public void Interact(InputAction.CallbackContext context)
            {
                if (context.started)
                    m_trigger.enabled = true;
            }

            private void InteractWithInteractableObject(GameObject obj)
            {
                Debug.Log(obj.name + " was interacted with.");

                // LOGIC HERE

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
}
