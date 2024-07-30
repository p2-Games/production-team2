///<summary>
/// Author: Halen
///
/// Tracks the status of the player.
///
///</summary>

using Millivolt.UI;
using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerStatus : MonoBehaviour
        {
            [SerializeField] private float m_maxHealth;
            private float m_currentHealth;
            [SerializeField] private PlayerHurtEffect m_playerHurtEffect;
            [SerializeField] private PlayerHealthBarUI m_playerHealthbar;


            private void Start()
            {
                m_currentHealth = m_maxHealth;
            }

            public void TakeDamage(float value)
            {
                m_currentHealth -= value;

                if (m_currentHealth <= 0)
                    Die();

                m_playerHealthbar.UpdateHealthBar(m_currentHealth, m_maxHealth);
                m_playerHurtEffect.ChangeVignetteAlpha(m_currentHealth, m_maxHealth);
            }

            public void Die()
            {
                Debug.Log("You are dead.");

                // LOGIC HERE
            }
        }
    }
}
