///<summary>
/// Author: Halen
///
/// Deals damage to the player while they are within a trigger.
///
///</summary>

using UnityEngine;
using PlayerStatus = Millivolt.Player.PlayerStatus;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace HazardObjects
        {
            public class DamageWires : HazardObject
            {
                private Collider[] m_triggers;

                public override bool isActive
                {
                    get { return m_isActive; }
                    set
                    {
                        foreach (Collider trigger in m_triggers)
                            trigger.enabled = value;
                        m_isActive = value;
                    }
                }

                private void Start()
                {
                    m_triggers = GetComponents<Collider>();
                }

                private void OnTriggerStay(Collider other)
                {
                    PlayerStatus player = other.GetComponent<PlayerStatus>();
                    if (player)
                        player.TakeDamage(m_damage * Time.deltaTime);
                }
            }
        }
    }
}
