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
        namespace EventObjects
        {
            [RequireComponent(typeof(Collider))]
            public class ObjectReceptacle : EventObject
            {
                [Header("Pickup Receptacle Details"), Tooltip("The name of the type of Object this receptacle accepts.")]
                [SerializeField] private string m_objectName;

                [Tooltip("The position an object will appear when it is removed from the receptacle.")]
                [SerializeField] private Transform m_respawnTransform;

                [Tooltip("Set a Prefab here to have it start with the object inside.")]
                [SerializeField] private LevelObject m_heldObject;

                [Tooltip("How long the receptacle waits after an object is removed from it before it checks for another object.")]
                [SerializeField] private float m_retrieveObjectTime;
                private float m_timer;

                public override bool canInteract => m_canInteract && m_isActive;

                private void Start()
                {
                    if (m_heldObject)
                    {
                        m_objectName = m_heldObject.name;
                        m_heldObject = Instantiate(m_heldObject);
                        m_heldObject.gameObject.SetActive(false);
                    }
                    isActive = m_heldObject != null;
                    if (m_togglesOnce)
                        m_canInteract = true;
                }

                private void Update()
                {
                    if (m_timer < m_retrieveObjectTime)
                        m_timer += Time.deltaTime;
                }

                // for items getting put in
                private void OnTriggerStay(Collider other)
                {
                    // if the receptacle cannot be interacted with, don't check for anything
                    if (!m_canInteract)
                        return;
                    
                    // if the receptacle already has an object in it, don't check for anything
                    if (m_heldObject)
                        return;

                    // if the retrieve object timer is not finished, don't check for anything
                    if (m_timer < m_retrieveObjectTime)
                        return;

                    // if its the correct object, accept the object
                    LevelObject obj = other.GetComponent<LevelObject>();
                    if (obj && obj.name == m_objectName)
                    {
                        // if the object has a spawn parent, then toggle whether it can still spawn the object
                        if (obj.spawnParent)
                            obj.spawnParent.SetCanSpawnObject(false);

                        // keep reference to obj
                        m_heldObject = obj;

                        // deactivate the object
                        m_heldObject.gameObject.SetActive(false);

                        ToggleState();
                    }
                }

                // taking objects out
                public override void Interact()
                {
                    base.Interact();

                    if (m_timer < m_retrieveObjectTime)
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

                    ToggleState();
                }

                private void ToggleState()
                {
                    // set the state of the object
                    if (m_togglesOnce)
                        m_canInteract = false;
                    m_timer = 0;
                    isActive = !m_isActive;
                }
            }
        }
    }
}