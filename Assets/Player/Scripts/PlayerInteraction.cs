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
    using LevelObjects.InteractableObjects;
    using LevelObjects.PickupObjects;

    namespace Player
    {
        using UI;

        namespace Interaction
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

                private GameObject m_closestObject;

                public InteractionState interactionState => m_state;

                public bool canInteract => m_state != InteractionState.Closed && m_interactTimer >= m_interactTime && m_closestObject;

                public string heldItemName => m_heldPickup.keyword;

                // methods
                private void Start()
                {
                    InitialiseTrigger();
                }

                private void Update()
                {
                    if (m_interactTimer < m_interactTime)
                        m_interactTimer += Time.deltaTime;
                }

                private void InitialiseTrigger()
                {
                    m_trigger = GetComponent<SphereCollider>();
                    m_trigger.isTrigger = true;
                }

                // interacting with objects
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
                    if (interactableObject && interactableObject.playerCanInteract)
                        interactableObject.Interact();
                    else
                    {
                        PickupObject pickupObject = m_closestObject.GetComponent<PickupObject>();
                        if (pickupObject && pickupObject.playerCanGrab)
                            GrabObject(m_closestObject.GetComponent<PickupObject>());
                    }
                    
                    m_interactTimer = 0;
                }

                private void OnTriggerEnter(Collider other)
                {
                    bool NewObjectIsCloserThanCurrent(Transform newObject)
                    {
                        if (!m_closestObject)
                            return true;

                        Vector3 centre = transform.position + m_trigger.bounds.center;
                        return Vector3.Distance(newObject.position, centre) < Vector3.Distance(m_closestObject.transform.position, centre);
                    }

                    bool ObjectIsInteractable(GameObject obj)
                    {
                        return obj.GetComponent<InteractableObject>() || obj.GetComponent<PickupObject>();
                    }

                    // if the object is interactable (pickup or interactable) AND is not the currently held object AND its closer than the other saved object, save it
                    if (ObjectIsInteractable(other.gameObject) && other.gameObject != m_heldPickup.gameObject && NewObjectIsCloserThanCurrent(other.gameObject.transform))
                    {
                        m_closestObject = other.gameObject;
                        m_interactionUI.UpdateDisplay();
                    }
                }

                private void OnTriggerExit(Collider other)
                {
                    if (other.gameObject == m_closestObject)
                    {
                        m_closestObject = null;
                        m_interactionUI.UpdateDisplay();
                    }
                }

                // picking up and dropping objects
                public void GrabObject(PickupObject obj)
                {
                    if (m_closestObject == obj.gameObject)
                    {
                        m_closestObject = null;
                        m_interactionUI.UpdateDisplay();
                    }

                    m_heldPickup = obj;
                    m_heldPickup.useGravity = false;
                    m_heldPickup.transform.SetParent(m_heldObjectOffset);
                    m_heldPickup.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                    m_state = InteractionState.Holding;
                }

                public void DropObject()
                {
                    m_heldPickup.useGravity = true;
                    m_heldPickup.transform.SetParent(null, true);
                    m_heldPickup = null;

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
                        Handles.DrawLine(transform.position + m_trigger.bounds.center, m_closestObject.transform.position);
                }
#endif
            }
        }
    }
}
