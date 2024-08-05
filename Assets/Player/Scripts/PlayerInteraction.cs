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

    namespace Player
    {
        public enum InteractionState
        {
            Open, // free to interact with and pick up objects
            Holding, // currently holding an object
            Closed // unable to interact with or pick up objects
        }
        
        public class PlayerInteraction : MonoBehaviour
        {
            // interaction state
            private InteractionState m_state;
            public InteractionState interactionState => m_state;

            public void SetInteractionState(InteractionState state)
            {
                if (m_state == InteractionState.Holding && state != InteractionState.Holding)
                m_state = state;
            }

            // player details
            [Tooltip("How many fixed update frames the interact trigger stays active for.\nThere are 50 fixed update frames each second by default.")]
            [SerializeField] private float m_interactFrames;
            private float m_frameCounter;
            public bool canInteract => m_frameCounter == 0 && m_state != InteractionState.Closed;

            [Header("Pickups"), SerializeField] private Transform m_pickupOffset;
            [SerializeField] private float m_pickupMaxSpeed;
            [SerializeField] private float m_pickupAcceleration;

            // components
            private Collider m_trigger;
            private Rigidbody m_heldPickup;

            // methods
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
                // interaction timer
                if (m_frameCounter == 0)
                    m_trigger.enabled = false;
                else
                    m_frameCounter--;

                // update picked up object position
                if (m_state == InteractionState.Holding)
                {
                    Vector3 diff = m_pickupOffset.position - m_heldPickup.transform.position;
                    float magnitudeSqr = diff.magnitude * diff.magnitude;
                    m_heldPickup.velocity = Vector3.MoveTowards(m_heldPickup.velocity,
                        diff.normalized * m_pickupMaxSpeed * magnitudeSqr,
                        m_pickupAcceleration * magnitudeSqr);
                }
            }

            // interacting with objects
            public void Interact(InputAction.CallbackContext context)
            {
                // if button pressed
                if (context.started)
                {
                    // if the player can interact
                    if (canInteract)
                    {
                        switch (m_state)
                        {
                            case InteractionState.Open:
                                m_trigger.enabled = true;
                                break;
                            case InteractionState.Holding:
                                DropObject();
                                break;
                        }

                        m_frameCounter = m_interactFrames;
                    }
                }
            }

            private void OnTriggerEnter(Collider other)
            {
                // check if the object is a pickup object
                if (other.tag == "Pickup")
                {
                    PickUpObject(other.transform);
                    m_trigger.enabled = false;
                }
                // otherwise check if object is interactable object
                else
                {
                    InteractableObject obj = other.GetComponent<InteractableObject>();
                    if (obj)
                    {
                        if (obj.playerCanInteract)
                        {
                            obj.Interact();
                            // disable the trigger after object has been found
                            m_trigger.enabled = false;
                        }
                    }
                }
            }

            // picking up and dropping objects
            public void PickUpObject(Transform obj)
            {
                m_heldPickup = obj.GetComponent<Rigidbody>();
                m_heldPickup.useGravity = false;

                m_state = InteractionState.Holding;
            }

            public void DropObject()
            {
                m_heldPickup.useGravity = true;
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

                Handles.DrawWireCube(m_pickupOffset.position, new Vector3(0.25f, 0.25f, 0.25f));
            }
#endif
        }
    }
}
