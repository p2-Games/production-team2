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

            [Tooltip("How many fixed update frames the interact trigger stays active for.\nThere are 50 fixed update frames each second.")]
            [SerializeField] private float m_interactFrames;
            private float m_frameCounter;

            private void Start()
            {
                m_trigger = GetComponent<SphereCollider>();
                m_trigger.isTrigger = true;
                m_trigger.enabled = false;
            }

            private void Update()
            {
                transform.localRotation = Quaternion.Euler(Camera.main.transform.eulerAngles.x, 0, 0);
            }

            private void FixedUpdate()
            {
                if (m_frameCounter == 0)
                    m_trigger.enabled = false;
                m_frameCounter--;
            }

            public void Interact(InputAction.CallbackContext context)
            {
                if (context.started)
                {
                    m_trigger.enabled = true;
                    m_frameCounter = m_interactFrames;
                }
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
