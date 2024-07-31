///<summary>
/// Author: Emily McDonald
///
/// This manages settings about the game
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace Utilities
	{
		public enum GameState
		{
			MENU,
			PAUSE,
			PLAYING,
			FINISH
		}

		public class GameManager : MonoBehaviour
		{
			[SerializeField] protected Vector3 m_gravity = new Vector3(0, -9.81f, 0);
			private GameState m_gameState;

			public GameState gameState
			{
				get { return m_gameState; }
				set { m_gameState = value; }
			}

			public Vector3 gravity
			{
				get { return m_gravity; }
				set { m_gravity = value; }
			}

            public void ChangeGravity(Vector3 newGravity)
            {
                m_gravity = newGravity;
            }

            private void Start()
            {
                
            }

            private void Update()
			{
				switch (m_gameState)
				{
					case GameState.MENU:
						break;
					case GameState.PAUSE:
						Time.timeScale = 0;
						break;
					case GameState.PLAYING:
						Time.timeScale = 1;
						break;
					case GameState.FINISH:
						break;
				}
			}
		}
	}
}
