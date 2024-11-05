///<summary>
/// Author: Halen
///
/// Kills the player when they touch an attached trigger.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    using Millivolt.LevelObjects.PickupObjects;
    using Player;

    namespace LevelObjects
    {
        public class Killzone : LevelObject
        {
            private void Start()
            {
                Collider collider = GetComponent<Collider>();
                collider.isTrigger = true;
            }

            private void OnTriggerEnter(Collider other)
            {
                // if inactive, don't kill
                if (!m_isActive)
                    return;

                // if the other object is the player, kill them
                if (other.GetComponent<PlayerModel>())
                    GameManager.Player.Status.TakeDamage(GameManager.Player.Status.maxHealth);
                // if the other object is a pickup, destroy it
                else if (other.TryGetComponent(out Interactable obj))
                {
                    if (obj.TryGetComponent(out PickupObject pickup))
                        pickup.Destroy();
                    else
                        Destroy(obj.gameObject);
                }
            }
        }
    }
}
