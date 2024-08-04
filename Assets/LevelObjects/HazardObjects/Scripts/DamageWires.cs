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
                [SerializeField] private GameObject m_particleObject;
                private Collider[] m_triggers;

                public override bool isActive
                {
                    get => base.isActive;
                    set
                    {
                        m_particleObject.SetActive(value);

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
                    if (m_isActive)
                    {
                        PlayerStatus player = other.GetComponent<PlayerStatus>();
                        if (player)
                            player.TakeDamage(m_damage * Time.deltaTime);
                    }
                }
            }
        }
    }
}
