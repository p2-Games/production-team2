///<summary>
/// Author: Emily
///
/// Check if you can interact with an object infront of you
///
///</summary>

using Millivolt.Player;
using UnityEngine;

namespace Millivolt
{
    using LevelObjects;
    using LevelObjects.PickupObjects;
    public enum InteractionState
    {
        Open, // free to interact with and pick up objects
        Holding, // currently holding an object
        Closed // unable to interact with or pick up objects
    }

    public class PlayerCheckInteractable : MonoBehaviour
	{
        [SerializeField] private GameObject m_interactUI;

        // interaction state
        private InteractionState m_state;
        public InteractionState interactionState => m_state;

        public void SetInteractionState(InteractionState state)
        {
            if (m_state == InteractionState.Holding && state != InteractionState.Holding)
                m_state = state;
        }

        private Collider m_trigger;
        [SerializeField] private PlayerInteraction m_interaction;

        private void Start()
        {
            m_trigger = GetComponent<SphereCollider>();
            m_trigger.isTrigger = true;
            m_interaction = FindObjectOfType<PlayerInteraction>();
            m_interactUI.SetActive(false);
        }

        private void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_interaction.interactionState == Player.InteractionState.Holding)
            {
                m_interactUI.SetActive(false);
                return;
            }

            // check if the object is a pickup object and if it can be picked up
            PickupObject pickupObj = other.GetComponent<PickupObject>();
            if (pickupObj && pickupObj.pickupType != PickupType.Immovable)
            {
                //PickUpObject(pickupObj);
                m_interactUI.SetActive(true);
            }
            // otherwise check if object is interactable object
            else
            {
                InteractableObject interactObj = other.GetComponent<InteractableObject>();
                if (interactObj)
                {
                    if (interactObj.playerCanInteract)
                    {
                        m_interactUI.SetActive(true);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // check if the object is a pickup object and if it can be picked up
            PickupObject pickupObj = other.GetComponent<PickupObject>();
            if (pickupObj && pickupObj.pickupType != PickupType.Immovable)
            {
                //PickUpObject(pickupObj);
                m_interactUI.SetActive(false);
            }
            // otherwise check if object is interactable object
            else
            {
                InteractableObject interactObj = other.GetComponent<InteractableObject>();
                if (interactObj)
                {
                    if (interactObj.playerCanInteract)
                    {
                        m_interactUI.SetActive(false);
                    }
                }
            }
        }
    }
}
