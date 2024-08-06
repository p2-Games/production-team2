///<summary>
/// Author: Halen
///
/// Accepts a type of pickup object.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        [RequireComponent(typeof(Collider))]
        public class PickupReceptacle : InteractableObject
        {
            [Header("Pickup Receptacle Details"), Tooltip("The keyword of the type of pickup this receptacle accepts.")]
            [SerializeField] private string m_pickupKeyword;

            private void OnTriggerEnter(Collider other)
            {
                PickupObject obj = other.GetComponent<PickupObject>();
                if (obj)
                {
                    // if its the correct pickup, do the things
                    if (obj.keyword == m_pickupKeyword)
                    {
                        // if the object has a spawn parent, then toggle whether it can still spawn the object
                        if (obj.spawnParent)
                            obj.spawnParent.SetCanSpawnObject(false);

                        // destroy the object
                        Destroy(obj.gameObject);

                        // toggle this receptacle's active state
                        isActive = !m_isActive;
                    }
                }    
            }
        }
    }
}
