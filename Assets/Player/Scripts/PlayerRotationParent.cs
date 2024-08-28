///<summary>
/// Author: Halen
///
/// Updates the position of the player's rotation parent for gravity things when necessary.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        [ExecuteInEditMode]
        public class PlayerRotationParent : MonoBehaviour
        {
            [SerializeField] private Transform m_player;

            private void Start()
            {
                if (!m_player)
                    m_player = transform.GetChild(0);
            }

            private void Update()
            {
                //if (transform.hasChanged)
                //{
                //    ResetPosition();
                //    transform.hasChanged = false;
                //}
            }

            public void ResetPosition()
            {
                transform.position = m_player.position;
                m_player.localPosition = Vector3.zero;
            }
        }
    }
}