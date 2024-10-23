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
    using Player.UI;
    using LevelObjects;
    using LevelObjects.PickupObjects;

    namespace Player
    {
        public enum InteractionState
        {
            Open, // free to interact with and pick up objects
            Holding, // currently holding an object
            Closed // unable to interact with or pick up objects
        }

        [System.Serializable]
        public enum InputType
        {
            Move,
            Look,
            Jump,
            Interact,
            Pause,
            INPUT_COUNT
        }

        [RequireComponent(typeof(Collider))]
        public class PlayerInteraction : MonoBehaviour
        {
            private Collider m_trigger;

            [SerializeField] private InteractionUI m_interactionUI;
            [SerializeField] private Transform m_heldObjectOffset;
            [Tooltip("How long the player has to wait between interacting with objects.")]
            [SerializeField] private float m_interactTime;
            [SerializeField] private bool m_drawGizmos = false;

            private InteractionState m_state;
            private PickupObject m_heldPickup;
            private float m_interactTimer;

            private Interactable m_closestObject;

            public GameObject heldObject
            {
                get
                {
                    if (m_heldPickup)
                        return m_heldPickup.gameObject;
                    else return null;
                }
            }
            public bool canInteract => m_state != InteractionState.Closed && m_interactTimer >= m_interactTime;

            private void Start()
            {
                InitialiseTrigger();
            }

            private void InitialiseTrigger()
            {
                m_trigger = GetComponent<SphereCollider>();
                m_trigger.isTrigger = true;
            }

            private void SetClosestObject(Interactable interactable)
            {
                m_closestObject = interactable;
                m_interactionUI.UpdateDisplay(interactable);
            }

            private void Update()
            {
                if (m_interactTimer < m_interactTime)
                    m_interactTimer += Time.deltaTime;

                // if the player has a held pickup
                if (m_heldPickup)
                {
                    // if the pickup exists but is disabled, it has been accepted by a receptacle
                    // so remove the reference and reset the interaction state
                    if (!m_heldPickup.gameObject.activeSelf)
                        DropObject();
                    // keep held object in front of player
                    else
                        m_heldPickup.rb.MovePosition(m_heldObjectOffset.position);
                }

                // if the current closest object gets accepted by a receptacle or destroyed, update this 
                if (m_interactionUI.isActive && (!m_closestObject || !m_closestObject.gameObject.activeSelf))
                {
                    SetClosestObject(null);
                }
            }

            /// <summary>
            /// Interacts with the closest Object to the player, as shown in the Interaction UI display.
            /// </summary>
            /// <param name="context"></param>
            public void Interact(InputAction.CallbackContext context)
            {
                if (!canInteract)
                    return;
                
                // if button pressed
                if (context.started)
                {
                    switch (m_state)
                    {
                        case InteractionState.Open:
                            if (m_closestObject)
                            {
                                m_closestObject.Interact(this);
                                SetClosestObject(null);
                            }
                            break;
                        case InteractionState.Holding:
                            DropObject();
                            break;
                    }
                }
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

                // if the player is holding something, stop
                if (m_state != InteractionState.Open)
                    return;

                // if the object does not have an interactable, stop
                if (other.gameObject.TryGetComponent(out Interactable interactable))
                {
                    // if the object is not interactable, stop
                    if (!interactable.canInteract)
                        return;

                    // if the object in the trigger is already the closest object, stop
                    if (m_closestObject == interactable)
                        return;
                }
                else
                    return;

                // if the object is closer than the saved current closest object
                if (NewObjectIsCloserThanCurrent(other.transform))
                {
                    SetClosestObject(interactable);
                }
            }

            private void OnTriggerExit(Collider other)
            {
                if (m_closestObject && other.gameObject == m_closestObject.gameObject)
                    SetClosestObject(null);
            }

            // picking up and dropping objects
            public void GrabObject(PickupObject obj)
            {
                // set the held object to the pickup
                m_heldPickup = obj;

                // stop the pickup's rigidbody from pulling it around
                m_heldPickup.rb.useGravity = false;

                // reset the state of the closest object and interaction display
                SetClosestObject(null);

                // ignore collision between the pickup layer and the player layer so that the
                // player and pickup don't collide.
                Physics.IgnoreLayerCollision(3, 9, true);

                // play pickup sound effect
                SFXController.Instance.PlayRandomSoundClip("ScrewPickUp", m_heldObjectOffset);

                m_state = InteractionState.Holding;
            }

            public void DropObject()
            {
                if (m_heldPickup)
                {
                    // let the pickup's rigidbody work again
                    m_heldPickup.rb.useGravity = true;

                    // stop controlling the pickup
                    m_heldPickup = null;
                }

                // let the player collide with pickups again
                // SHIT
                Physics.IgnoreLayerCollision(3, 9, false);

                m_state = InteractionState.Open;
            }

            public void ResetInteraction()
            {
                DropObject();
                m_interactTimer = 0f;
                SetClosestObject(null);
            }

#if UNITY_EDITOR
            private void OnDrawGizmos()
            {
                if (!m_drawGizmos)
                    return;
                
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
