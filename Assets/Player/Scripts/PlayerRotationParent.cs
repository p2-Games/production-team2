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
            private Transform m_player;
            
            private void Start()
            {
                m_player = transform.GetChild(0);
            }

            private void Update()
            {
                if (transform.hasChanged)
                {
                    transform.position = m_player.position;
                    m_player.localPosition = Vector3.zero;
                    transform.hasChanged = false;
                }
            }
        }
    }
}
