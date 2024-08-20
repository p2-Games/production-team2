///<summary>
/// Author: Halen
///
/// Accepts a type of pickup object.
///
///</summary>

using Millivolt.Player;
using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace PickupObjects
        {
            [RequireComponent(typeof(Collider))]
            public class PickupReceptacle : InteractableObject
            {
                [Header("Pickup Receptacle Details"), Tooltip("The keyword of the type of pickup this receptacle accepts.")]
                [SerializeField] private string m_pickupKeyword;

                [Tooltip("The position an object will appear when it is removed from the receptacle.")]
                [SerializeField] private Transform m_respawnObjectTransform;

                private PickupObject m_heldObject;

                // for items getting put in
                private void OnTriggerEnter(Collider other)
                {
                    // if the receptacle is inactive OR it already has an object in it, don't check for anything
                    if (m_timer > 0 || m_heldObject)
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

                // taking objects out
                public override void Interact()
                {
                    // if the receptacle can have objects taken out of it and it currently is holding an object
                    if (!m_staysActive && m_heldObject)
                    {
                        // reactivate the held object
                        m_heldObject.gameObject.SetActive(true);

                        // set position and rotation to that of the respawn transform
                        m_heldObject.transform.position = m_respawnObjectTransform.position;
                        m_heldObject.transform.rotation = m_respawnObjectTransform.rotation;

                        // re-allow its spawn parent to spawn it again if it has one
                        if (m_heldObject.spawnParent)
                            m_heldObject.spawnParent.SetCanSpawnObject(true);

                        // give the object to the player
                        GameObject.FindWithTag("Player").GetComponentInChildren<PlayerInteraction>().PickUpObject(m_heldObject);

                        // remove reference to the object
                        m_heldObject = null;

                        // set the inactive timer so it won't be picked up again instantly
                        m_timer = m_interactTime;
                        isActive = !m_isActive;
                    }
                }
            }
        }
    }
}
