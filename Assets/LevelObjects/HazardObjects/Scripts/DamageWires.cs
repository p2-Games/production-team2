///<summary>
/// Author: Halen
///
/// Deals damage to the player while they are within a trigger.
///
///</summary>

using UnityEngine;
using UnityEditor;
using PlayerStatus = Millivolt.Player.PlayerStatus;
using Millivolt.Player;

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

                private void OnTriggerEnter(Collider other)
                {
                    if (m_isActive)
                    {
                        PlayerStatus player = other.GetComponentInChildren<PlayerStatus>();
                        if (player)
                        {
                            player.TakeDamage(m_damage * Time.deltaTime);
                            player.PlayerKnockback(this);
                        }
                    }
                }

                private void OnCollisionEnter(Collision other)
                {
                    if (m_isActive)
                    {
                        PlayerStatus player = other.gameObject.GetComponentInChildren<PlayerStatus>();
                        if (player)
                        {
                            player.TakeDamage(m_damage * Time.deltaTime);
                            player.PlayerKnockback(this);
                        }
                    }
                }
            }
        }
    }
}
