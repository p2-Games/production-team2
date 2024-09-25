///<summary>
/// Author: Halen
///
/// Kills the player when they touch an attached trigger.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    using Player;
    using LevelObjects.PickupObjects;

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
                    GameManager.PlayerStatus.TakeDamage(GameManager.PlayerStatus.maxHealth);
                // if the other object is a pickup, destroy it
                else
                {
                    PickupObject pickup = other.GetComponent<PickupObject>();
                    if (pickup)
                        Destroy(other.gameObject);
                }
            }
        }
    }
}
