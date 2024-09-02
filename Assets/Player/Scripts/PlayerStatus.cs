///<summary>
/// Author: Halen, Emily
///
/// Tracks the status of the player.
/// Calls functions related to the player, such as UI updates and call respawn for death
///
///</summary>

using Cinemachine;
using Millivolt.Player.UI;
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
            public float maxHealth => m_maxHealth;
            private float m_currentHealth;

            [Header("LevelData Reference")]
            LevelManager m_levelManager;

            [Header("Health Regen Properties")]
            [Tooltip("This will be the number of seconds after taking damage that the player will begine to regen health")]
            [SerializeField] private float m_healthRegenTime;
            [Tooltip("How often health will be given back to the player")]
            [SerializeField] private float m_healthRegenRate;
            [Tooltip("The amount of health that will be restored by the regen rate")]
            [SerializeField] private float m_healthRegenAmount;
            private Coroutine m_regen;

            [Header("Hurt and Death effect references")]
            [Tooltip("This needs the screen effect material for the hurt effect")]
            [SerializeField] private Material m_staticVignette;

            private GameObject m_deathCanvas;

            [Header("Respawn Properties")]
            [SerializeField] private float m_respawnTime;

            private void Start()
            {
                m_currentHealth = m_maxHealth;
                UpdateVignetteEffect();
                m_levelManager = FindObjectOfType<LevelManager>();
                //Theres no conversion to GameObject for some reason so I did a hold variable for now </3
                PlayerDeathUI hold = (PlayerDeathUI)FindObjectOfType(typeof(PlayerDeathUI), true);
                m_deathCanvas = hold.gameObject;
            }

            public void TakeDamage(float value)
            {
                if (m_regen != null)
                    StopCoroutine(m_regen);

                m_currentHealth -= value;

                UpdateVignetteEffect();

                m_regen = StartCoroutine(RegenHealth());
                if (m_currentHealth <= 0)
                    Die();
            }

            /// <summary>
            /// This will be called once the players health has hit 0, thiswill load the player to their last checkpoint and give them full health
            /// </summary>
            public void Die()
            {
                Debug.Log("You are dead.");

                // LOGIC HERE
                m_currentHealth = m_maxHealth;
                UpdateVignetteEffect();
                m_deathCanvas.SetActive(true);
                Invoke(nameof(Respawn), m_respawnTime);
            }

            private void Respawn()
            {
                m_levelManager.SpawnPlayer();
            }

            /// <summary>
            /// IEnumerator for gradually giving the player back health as a regen effect
            /// </summary>
            /// <returns></returns>
            IEnumerator RegenHealth()
            {
                yield return new WaitForSeconds(m_healthRegenTime);
                while(m_currentHealth < m_maxHealth)
                {
                    m_currentHealth += m_healthRegenAmount;
                    UpdateVignetteEffect();
                    yield return new WaitForSeconds(m_healthRegenRate);
                }
                if (m_currentHealth > m_maxHealth)
                    m_currentHealth = m_maxHealth;
            }

            private void UpdateVignetteEffect()
            {
                float effectAmount = 1 - (m_currentHealth / m_maxHealth);

                if (effectAmount > 0.1)
                {
                    m_staticVignette.SetFloat("_VignetteIntensity", effectAmount);
                }
                else
                    m_staticVignette.SetFloat("_VignetteIntensity", 0);
            }
        }
    }
}
