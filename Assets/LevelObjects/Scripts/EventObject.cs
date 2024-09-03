///<summary>
/// Author: Halen
///
/// Level Object that can't be Interacted with by the Player but still has Events.
///
///</summary>

using UnityEngine;
using UnityEngine.Events;

namespace Millivolt
{
    using Player;

    namespace LevelObjects
    {
        namespace EventObjects
        {
            public abstract class EventObject : LevelObject
            {
                [Header("Event Object Details"), Tooltip("If true, the state of the object can change.")]
                [SerializeField] protected bool m_canInteract = true;

                [Tooltip("If true, the object can only be interacted with once.")]
                [SerializeField] protected bool m_togglesOnce = false;

                [Tooltip("Filter for what can interact with this object.\n" +
                        "Accepts System Types (class names) and Tags.")]
                [SerializeField] protected string[] m_interactionFilter = { "Player", typeof(LevelObject).Name };

                [Tooltip("The events that will occur when the object is set active.")]
                [SerializeField] protected UnityEvent m_activateEvents;

                [Tooltip("The events that will occur when the object is set inactive.")]
                [SerializeField] protected UnityEvent m_deactivateEvents;

                public bool canInteract => m_canInteract;

                private static PlayerInteraction m_playerInteraction;

                private void Start()
                {
                    if (!m_playerInteraction)
                        m_playerInteraction = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerInteraction>();
                }

                protected bool CanTrigger(GameObject obj)
                {
                    if (obj == m_playerInteraction.heldObject)
                        return false;

                    foreach (string type in m_interactionFilter)
                    {
                        if (obj.tag == type || obj.GetComponent(type))
                            return true;
                    }
                    return false;
                }

                public virtual void Interact()
                {
                    if (!m_canInteract)
                        return;
                }
            }
        }
    }
}
