///<summary>
/// Author: Halen
///
/// Handles interactions between the player and objects that can be interacted with in levels.
/// If an InteractableObject is detected by this script, it interacts with it accordingly.
///
///</summary>

using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Millivolt
{
    using LevelObjects;
    using LevelObjects.InteractableObjects;
    using Player.UI;

    namespace Player
    {
        public enum InteractionState
        {
            Open, // free to interact with and pick up objects
            Holding, // currently holding an object
            Closed // unable to interact with or pick up objects
        }

        [RequireComponent(typeof(Collider))]
        public class PlayerInteraction : MonoBehaviour
        {
            private Collider m_trigger;

            [SerializeField] private InteractionUI m_interactionUI;
            [SerializeField] private Transform m_heldObjectOffset;
            [Tooltip("How long the player has to wait between interacting with objects.")]
            [SerializeField] private float m_interactTime;

            private InteractionState m_state;
            private PickupObject m_heldPickup;
            private float m_interactTimer;

            private LevelObject m_closestObject;

            public bool canInteract => m_state != InteractionState.Closed && m_interactTimer >= m_interactTime && m_closestObject;

            private void Start()
            {
                InitialiseTrigger();
            }

            private void Update()
            {
                if (m_interactTimer < m_interactTime)
                    m_interactTimer += Time.deltaTime;

                // keep held object in front of player
                if (m_heldPickup)
                    m_heldPickup.rb.MovePosition(m_heldObjectOffset.position);

                // for when pickup object gets disabled
                if (!m_closestObject.gameObject.activeSelf)
                {
                    m_closestObject = null;
                    m_interactionUI.UpdateDisplay(false, m_closestObject);
                }
            }

            private void InitialiseTrigger()
            {
                m_trigger = GetComponent<SphereCollider>();
                m_trigger.isTrigger = true;
            }

            /// <summary>
            /// Interacts with the closest Object to the player, as shown in the Interaction UI display.
            /// </summary>
            /// <param name="context"></param>
            public void Interact(InputAction.CallbackContext context)
            {
                // if button pressed
                if (context.started)
                {
                    switch (m_state)
                    {
                        case InteractionState.Open:
                            if (canInteract)
                            {
                                InteractWithClosestObject();
                            }
                            break;
                        case InteractionState.Holding:
                            DropObject();
                            break;
                    }
                }
            }

            private void InteractWithClosestObject()
            {
                InteractableObject interactableObject = m_closestObject.GetComponent<InteractableObject>();
                if (interactableObject && interactableObject.canInteract)
                    interactableObject.Interact();
                else
                {
                    PickupObject pickupObject = m_closestObject.GetComponent<PickupObject>();
                    if (pickupObject && pickupObject.playerCanGrab)
                        GrabObject(m_closestObject.GetComponent<PickupObject>());
                }

                m_interactTimer = 0;
            }

            private void OnTriggerStay(Collider other)
            {
                bool NewObjectIsCloserThanCurrent(Transform newObject)
                {
                    if (!m_closestObject)
                        return true;

                    Vector3 centre = transform.position + m_trigger.bounds.center;
                    return Vector3.Distance(newObject.position, centre) < Vector3.Distance(m_closestObject.transform.position, centre);
                }

                // if the player is not holding something
                if (m_state != InteractionState.Open)
                    return;

                // if the object in the trigger is not already the closest object
                if (m_closestObject && m_closestObject.gameObject == other.gameObject)
                    return;

                // if the object is interactable or a pickup
                InteractableObject interactable = other.gameObject.GetComponent<InteractableObject>();
                PickupObject pickup = other.gameObject.GetComponent<PickupObject>();
                if (!interactable && !pickup)
                    return;

                if (interactable && !interactable.canInteract)
                    return;

                // if the object is not an immovable pickup object
                if (pickup && !pickup.playerCanGrab)
                    return;

                // if the object can is closer than the saved current closest object
                if (NewObjectIsCloserThanCurrent(other.gameObject.transform))
                {
                    m_closestObject = other.gameObject.GetComponent<LevelObject>();
                    m_interactionUI.UpdateDisplay(true, m_closestObject);
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (m_closestObject && other.gameObject == m_closestObject.gameObject)
                {
                    m_closestObject = null;
                    m_interactionUI.UpdateDisplay(false, m_closestObject);
                }
            }

            // picking up and dropping objects
            public void GrabObject(PickupObject obj)
            {
                m_heldPickup = obj;
                m_heldPickup.rb.useGravity = false;
                m_closestObject = null;
                m_interactionUI.UpdateDisplay(false, m_closestObject);
                Physics.IgnoreLayerCollision(3, 9, true);

                m_state = InteractionState.Holding;
            }

            public void DropObject()
            {
                m_heldPickup.rb.useGravity = true;
                m_heldPickup = null;
                Physics.IgnoreLayerCollision(3, 9, false);

                m_state = InteractionState.Open;
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                switch (m_state)
                {
                    case InteractionState.Open:
                        Handles.color = Color.green;
                        break;
                    case InteractionState.Holding:
                        Handles.color = Color.blue;
                        break;
                    case InteractionState.Closed:
                        Handles.color = Color.red;
                        break;
                }

                if (m_heldObjectOffset)
                    Handles.DrawWireCube(m_heldObjectOffset.position, Vector3.one / 4f);

                if (m_closestObject)
                    Handles.DrawLine(transform.position, m_closestObject.transform.position);
            }
#endif
        }
    }
}
