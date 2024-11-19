///<summary>
/// Author: Halen
///
/// Handles interactions between the player and objects that can be interacted with in levels.
/// If an Interactable is detected by this script, it interacts with it accordingly.
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
            [SerializeField, Min(0)] private float m_pickupDropDistance;

            [SerializeField] private bool m_drawGizmos = false;

            private InteractionState m_state = InteractionState.Closed;

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
            public bool canInteract => m_state != InteractionState.Closed && m_interactTimer >= m_interactTime && Time.timeScale > 0;

            private void Start()
            {
                InitialiseTrigger();
                ResetInteraction();
            }

            private void InitialiseTrigger()
            {
                m_trigger = GetComponent<SphereCollider>();
                m_trigger.isTrigger = true;
            }

            /// <summary>
            /// If the PlayerInteraction logic should be active or inactive.
            /// </summary>
            /// <param name="value"></param>
            public void SetInteractionState(bool value)
            {
                if (value)
                    m_state = InteractionState.Open;
                else
                    ResetInteraction();
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
                    // keep held object held in front of player
                    float distance = Vector3.Distance(m_heldObjectOffset.position, m_heldPickup.transform.position);
                    m_heldPickup.rb.AddForce((m_heldObjectOffset.position - m_heldPickup.transform.position).normalized * 10000f * distance * Time.deltaTime);

                    // if pickup is too far away from the player
                    if (Vector3.Distance(m_heldPickup.transform.position, transform.position) > m_pickupDropDistance)
                        DropObject();
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
                    // if the behaviour is not enabled, stop
                    if (!interactable.enabled)
                        return;
                    
                    // if the object is not interactable, stop
                    if (!interactable.canInteract)
                        return;

                    // if the object in the trigger is already the closest object, stop
                    if (m_closestObject == interactable)
                        return;

                    // if the object is closer than the saved current closest object
                    if (NewObjectIsCloserThanCurrent(other.transform))
                    {
                        SetClosestObject(interactable);
                    }
                }
                else
                    return;               
            }

            private void OnTriggerExit(Collider other)
            {
                if (m_closestObject && other.gameObject == m_closestObject.gameObject)
                    SetClosestObject(null);
            }

            // picking up and dropping objects
            public void GrabObject(PickupObject obj)
            {
                // Play grab animation
                GameManager.Player.Animation.SetTriggerParameter("Grab");
                GameManager.Player.Animation.PassBoolParameter("IsHolding", true);
                
                // set the held object to the pickup
                m_heldPickup = obj;

                // stop the pickup's rigidbody from pulling it around
                m_heldPickup.rb.useGravity = false;

                // change drag
                m_heldPickup.rb.drag = 10;
                m_heldPickup.rb.angularDrag = 2;

                // reset pickup velocity
                m_heldPickup.rb.velocity = Vector3.zero;

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
                // remove the held pickup                
                if (m_heldPickup)
                {
                    // let the pickup's rigidbody work again
                    m_heldPickup.rb.useGravity = true;

                    // change drag
                    m_heldPickup.rb.drag = 1;
                    m_heldPickup.rb.angularDrag = 0;

                    // give the dropped object a velocity
                    m_heldPickup.rb.velocity = GameManager.Player.Controller.rb.velocity * 1.2f;

                    // stop controlling the pickup
                    m_heldPickup = null;

                    // let the player collide with pickups again
                    // SHIT but works
                    Physics.IgnoreLayerCollision(3, 9, false);

                    // in case player is mid pick up, let them move again
                    // and do animator state machine logic for dropping the object
                    GameManager.Player.Controller.SetCanMove(CanMoveType.Pickup, true);
                    GameManager.Player.Animation.SetTriggerParameter("Drop");
                    GameManager.Player.Animation.PassBoolParameter("IsHolding", false);
                    m_state = InteractionState.Open;
                }
            }

            public void ResetInteraction()
            {
                m_state = InteractionState.Closed;

                // reset held pickup
                if (m_heldPickup)
                {
                    // let the pickup's rigidbody work again
                    m_heldPickup.rb.useGravity = true;

                    // stop controlling the pickup
                    m_heldPickup = null;
                }

                // let the player collide with pickups again
                // SHIT but works
                Physics.IgnoreLayerCollision(3, 9, false);

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

                // pickup drop distance
                Handles.matrix = transform.localToWorldMatrix;
                Handles.color = Color.green;
                Vector3 position = Vector3.zero;

                Handles.DrawWireDisc(position, Vector3.right, m_pickupDropDistance);
                Handles.DrawWireDisc(position, Vector3.up, m_pickupDropDistance);
                Handles.DrawWireDisc(position, Vector3.forward, m_pickupDropDistance);

                if (Camera.current.orthographic)
                {
                    Vector3 normal = position - Handles.inverseMatrix.MultiplyVector(Camera.current.transform.forward);
                    float sqrMagnitude = normal.sqrMagnitude;
                    float num0 = m_pickupDropDistance * m_pickupDropDistance;
                    Handles.DrawWireDisc(position - num0 * normal / sqrMagnitude, normal, m_pickupDropDistance);
                }
                else
                {
                    Vector3 normal = position - Handles.inverseMatrix.MultiplyPoint(Camera.current.transform.position);
                    float sqrMagnitude = normal.sqrMagnitude;
                    float num0 = m_pickupDropDistance * m_pickupDropDistance;
                    float num1 = num0 * num0 / sqrMagnitude;
                    float num2 = Mathf.Sqrt(num0 - num1);
                    Handles.DrawWireDisc(position - num0 * normal / sqrMagnitude, normal, num2);
                }

            }
#endif
        }
    }
}
