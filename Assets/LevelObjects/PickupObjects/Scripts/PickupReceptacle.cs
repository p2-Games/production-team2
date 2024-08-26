///<summary>
/// Author: Halen
///
/// Accepts a type of pickup object.
///
///</summary>

using UnityEngine;
using UnityEngine.Events;

namespace Millivolt
{
    //using Player.Interaction;

    namespace LevelObjects
    {
        using InteractableObjects;

        namespace PickupObjects
        {
            [RequireComponent(typeof(Collider))]
            public class PickupReceptacle : ToggleObject
            {
                public override bool isActive
                {
                    get => base.isActive;
                    set
                    {
                        // do nothing if the object should stay active and is currently active
                        if (m_staysActive && m_isActive)
                            return;

                        // if the active state is being changed, do all events
                        foreach (UnityEvent<bool> _event in m_activateEvents)
                            _event.Invoke(value);

                        // drop object if changing to inactive
                        if (!value)
                            DropObject();

                        // save state
                        m_isActive = value;
                    }
                }

                [Header("Pickup Receptacle Details"), Tooltip("The keyword of the type of pickup this receptacle accepts.")]
                [SerializeField] private string m_pickupKeyword;

                [Tooltip("The position an object will appear when it is removed from the receptacle.")]
                [SerializeField] private Transform m_respawnObjectTransform;

                private PickupObject m_heldObject;

                protected override void Update()
                {
                    base.Update();

                    // check if the held object has been destroyed or respawned
                    if (m_isActive && !m_heldObject)
                        isActive = false;
                }

                // for items getting put in
                private void OnTriggerEnter(Collider other)
                {
                    // if the receptacle is inactive OR it already has an object in it, don't check for anything
                    if (m_interactTimer < m_interactTime || m_heldObject)
                        return;

                    PickupObject obj = other.GetComponent<PickupObject>();
                    if (obj)
                    {
                        // if its the correct pickup, do the things
                        if (obj.keyword == m_pickupKeyword)
                        {
                            // if the object has a spawn parent, then toggle whether it can still spawn the object
                            if (obj.spawnParent)
                                obj.spawnParent.SetCanSpawnObject(false);

                            // keep reference to obj
                            m_heldObject = obj;

                            // deactivate the object
                            m_heldObject.gameObject.SetActive(false);

                            // toggle this receptacle's active state
                            isActive = !m_isActive;
                        }
                    }
                }

                private void DropObject()
                {
                    if (!m_heldObject)
                        return;
                    
                    // reactivate the held object
                    m_heldObject.gameObject.SetActive(true);

                    // set position and rotation to that of the respawn transform
                    m_heldObject.transform.SetPositionAndRotation(m_respawnObjectTransform.position, m_respawnObjectTransform.rotation);

                    // re-allow its spawn parent to spawn it again if it has one
                    if (m_heldObject.spawnParent)
                        m_heldObject.spawnParent.SetCanSpawnObject(true);

                    // give the object to the player
                    //GameObject.FindWithTag("Player").GetComponentInChildren<PlayerInteraction>().GrabObject(m_heldObject);

                    // remove reference to the object
                    m_heldObject = null;
                }
            }
        }
    }
}
