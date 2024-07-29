///<summary>
/// Author: Halen
///
/// Tracks the status of the player.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        public class PlayerStatus : MonoBehaviour
        {
            [SerializeField] private float m_maxHealth;
            private float m_currentHealth;

            private void Start()
            {
                m_currentHealth = m_maxHealth;
            }

            public void TakeDamage(float value)
            {
                m_currentHealth -= value;

                if (m_currentHealth <= 0)
                    Die();
            }

            public void Die()
            {
                Debug.Log("You are dead.");

                // LOGIC HERE
            }
        }
    }
}
