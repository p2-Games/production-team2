///<summary>
/// Author: Halen
///
/// Base class for objects that can damage or kill the player.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace HazardObjects
        {
            public abstract class HazardObject : LevelObject
            {               
                [Header("Hazard Details"), Tooltip("The base damage that the hazard deals to the player.")]
                [SerializeField] protected float m_damage;

                protected void DealDamage(Player.PlayerStatus player, float value)
                {
                    player.TakeDamage(value);
                }
            }
        }
    }
}
