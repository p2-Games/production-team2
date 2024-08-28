///<summary>
/// Author: Halen
///
/// Accepts a type of Level Object.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace InteractableObjects
        {
            [RequireComponent(typeof(Collider))]
            public class ObjectReceptacle : ToggleObject
            {
                [Header("Pickup Receptacle Details"), Tooltip("The name of the type of pickup this receptacle accepts.")]
                [SerializeField] private string m_pickupName;

                [Tooltip("The position an object will appear when it is removed from the receptacle.")]
                [SerializeField] private Transform m_respawnTransform;

                private LevelObject m_heldObject;

                // for items getting put in
                private void OnTriggerEnter(Collider other)
                {
                    // if the receptacle is inactive OR it already has an object in it, don't check for anything
                    if (m_interactTimer < m_interactDelay || m_playerCanInteract)
                        return;

                    LevelObject obj = other.GetComponent<LevelObject>();
                    if (obj)
                    {
                        // if its the correct pickup, accept the object
                        if (obj.name == m_pickupName)
                        {
                            // if the object has a spawn parent, then toggle whether it can still spawn the object
                            if (obj.spawnParent)
                                obj.spawnParent.SetCanSpawnObject(false);

                            // keep reference to obj
                            m_heldObject = obj;

                            // deactivate the object
                            m_heldObject.gameObject.SetActive(false);

                            // toggle this receptacle's active state
                            m_playerCanInteract = true;
                            isActive = !m_isActive;
                        }
                    }
                }

                // taking objects out
                public override void Interact()
                {
                    // if the receptacle only toggles once and has an object, return
                    if (m_togglesOnce && m_playerCanInteract)
                        return;

                    // reactivate the held object
                    m_heldObject.gameObject.SetActive(true);

                    // set position and rotation to that of the respawn transform
                    m_heldObject.transform.SetLocalPositionAndRotation(m_respawnTransform.position, m_respawnTransform.rotation);

                    // re-allow its spawn parent to spawn it again if it has one
                    if (m_heldObject.spawnParent)
                        m_heldObject.spawnParent.SetCanSpawnObject(true);

                    // remove reference to the object
                    m_heldObject = null;

                    // set the inactive timer so it won't be picked up again instantly
                    m_interactTimer = m_interactDelay;

                    // set the state of the object
                    m_playerCanInteract = false;
                    isActive = !m_isActive;
                }
            }
        }
    }
}