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
    using InteractableObject = LevelObjects.InteractableObject;

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
                {
                    m_trigger.enabled = false;
                }
                m_frameCounter--;
            }

            public void Interact(InputAction.CallbackContext context)
            {
                if (context.started && !m_trigger.enabled)
                {
                    m_trigger.enabled = true;
                    m_frameCounter = m_interactFrames;
                }
            }

            private void OnTriggerEnter(Collider other)
            {
                InteractableObject obj = other.GetComponent<InteractableObject>();
                if (obj)
                    InteractWithObject(obj);
            }

            private void InteractWithObject(InteractableObject obj)
            {
                obj.Interact();

                m_trigger.enabled = false;
            }
        }
    }
}
