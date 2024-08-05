///<summary>
/// Author: Halen, Emily
///
/// Tracks the status of the player.
/// Calls functions related to the player, such as UI updates and call respawn for death
///
///</summary>

using Millivolt.UI;
using Millivolt.Utilities;
using System.Collections;
using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerStatus : MonoBehaviour
        {
            [SerializeField] private float m_maxHealth;
            private float m_currentHealth;

            [Header("Health Canvas References")]
            [SerializeField] private PlayerHurtEffect m_playerHurtEffect;
            [SerializeField] private PlayerHealthBarUI m_playerHealthbar;

            [Header("LevelData Reference")]
            [SerializeField] LevelData m_lvlData;

            [Header("Health Regen Properties")]
            [Tooltip("This will be the number of seconds after taking damage that the player will begine to regen health")]
            [SerializeField] private float m_healthRegenTime;
            [Tooltip("How often health will be given back to the player")]
            [SerializeField] private float m_healthRegenRate;
            [Tooltip("The amount of health that will be restored by the regen rate")]
            [SerializeField] private float m_healthRegenAmount;
            private Coroutine m_regen;

            private void Start()
            {
                m_currentHealth = m_maxHealth;
                m_lvlData = FindObjectOfType<LevelData>();
            }

            public void TakeDamage(float value)
            {
                if (m_regen != null)
                    StopCoroutine(m_regen);

                m_currentHealth -= value;

                if (m_currentHealth <= 0)
                    Die();

                m_playerHealthbar.UpdateHealthBar(m_currentHealth, m_maxHealth);
                m_playerHurtEffect.ChangeVignetteAlpha(m_currentHealth, m_maxHealth);

                m_regen = StartCoroutine(RegenHealth());
            }

            public void Die()
            {
                Debug.Log("You are dead.");

                // LOGIC HERE
                m_currentHealth = m_maxHealth;
                m_playerHealthbar.UpdateHealthBar(m_currentHealth, m_maxHealth);
                m_playerHurtEffect.ChangeVignetteAlpha(m_currentHealth, m_maxHealth);
                m_lvlData.GetActiveCheckpoint().RespawnPlayer(gameObject);

            }

            /*private void RegenHealth()
            {
                m_currentHealth += m_healthRegenAmount;
                m_playerHealthbar.UpdateHealthBar(m_currentHealth, m_maxHealth);
                m_playerHurtEffect.ChangeVignetteAlpha(m_currentHealth, m_maxHealth);
                Invoke("RegenHealth", m_healthRegenRate);
            }*/

            IEnumerator RegenHealth()
            {
                yield return new WaitForSeconds(m_healthRegenTime);
                while(m_currentHealth < m_maxHealth)
                {
                    m_currentHealth += m_healthRegenAmount;
                    m_playerHealthbar.UpdateHealthBar(m_currentHealth, m_maxHealth);
                    m_playerHurtEffect.ChangeVignetteAlpha(m_currentHealth, m_maxHealth);
                    yield return new WaitForSeconds(m_healthRegenRate);
                }
                if (m_currentHealth > m_maxHealth)
                    m_currentHealth = m_maxHealth;
            }
        }
    }
}
