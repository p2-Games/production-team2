///<summary>
/// Author: Halen
///
/// Level Object that can't be Interacted with by the Player but still has Events.
///
///</summary>

using Millivolt.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace EventObjects
        {
            public abstract class EventObject : LevelObject
            {
                public override bool isActive
                {
                    get { return m_isActive; }
                    set
                    {
                        if (value)
                        {
                            m_activateEvents.Invoke();

                            // play sound effect
                            if (m_soundClipCollectionName != string.Empty && m_activateSoundClipName != string.Empty)
                                SFXController.Instance.PlaySoundClip(m_soundClipCollectionName, m_activateSoundClipName, transform);
                        }
                        else
                        {
                            m_deactivateEvents.Invoke();

                            // play sound effect
                            if (m_soundClipCollectionName != string.Empty && m_deactivateSoundClipName != string.Empty)
                                SFXController.Instance.PlaySoundClip(m_soundClipCollectionName, m_deactivateSoundClipName, transform);
                        }

                        if (m_togglesOnce)
                            m_canInteract = false;

                        m_isActive = value;
                    }
                }

                [Header("Event Object Details"), Tooltip("If true, the state of the object can change.")]
                [SerializeField] protected bool m_canInteract = true;

                [Tooltip("If true, the object can only be interacted with once.")]
                [SerializeField] protected bool m_togglesOnce = false;

                [Tooltip("Filter for what can interact with this object.\n" +
                        "Accepts System Types (class names) and Tags.\n" +
                        "If a filter begins with '!', then it will be ignored instead of accepted.")]
                [SerializeField] protected string[] m_interactionFilter = { typeof(PlayerModel).Name, typeof(LevelObject).Name };

                [Tooltip("The events that will occur when the object is set active.")]
                [SerializeField] protected UnityEvent m_activateEvents;

                [Tooltip("The events that will occur when the object is set inactive.")]
                [SerializeField] protected UnityEvent m_deactivateEvents;

                [Header("Sound Effects")]
                [SerializeField] protected string m_soundClipCollectionName;
                [SerializeField] protected string m_activateSoundClipName;
                [SerializeField] protected string m_deactivateSoundClipName;

                public virtual bool canInteract => m_canInteract;

                protected bool CanTrigger(GameObject obj)
                {
                    if (obj == GameManager.PlayerInteraction.heldObject)
                        return false;

                    foreach (string type in m_interactionFilter)
                    {
                        if (type[0] == '!')
                        {
                            string actualType = type.Substring(1);
                            if (obj.tag == actualType || obj.GetComponent(actualType))
                                return false;
                        }
                        else if (obj.tag == type || obj.GetComponent(type))
                            return true;
                    }
                    return false;
                }

                public virtual void Interact()
                {
                    if (!canInteract)
                        return;
                }
            }
        }
    }
}