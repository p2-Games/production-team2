///<summary>
/// Author: Halen
///
/// Kills the player when they touch an attached trigger.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    using PlayerStatus = Player.PlayerStatus;

    namespace LevelObjects
    {
        public class Killzone : LevelObject
        {
            private Collider m_collider;

            private void Start()
            {
                InitialiseCollider();
            }

            private void InitialiseCollider()
            {
                m_collider = GetComponent<Collider>();
                m_collider.isTrigger = true;
            }

            private void OnTriggerEnter(Collider other)
            {
                PlayerStatus player = other.GetComponentInChildren<PlayerStatus>();
                if (player)
                    player.TakeDamage(player.maxHealth);
            }
        }
    }
}
