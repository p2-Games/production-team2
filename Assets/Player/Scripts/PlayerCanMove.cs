///<summary>
/// Author: Halen
///
/// Holds the class for the complex Player 'can move' check.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
    namespace Player
    {
        public enum CanMoveType
        {
            LevelObject,
            Cutscene,
            Gravity,
            Dead,
            Menu,
            Pickup,
            COUNT
        }

        public class PlayerCanMove
        {
            public PlayerCanMove()
            {
                m_canMoves = new bool[(int)CanMoveType.COUNT];

                for (int c = 0; c < (int)CanMoveType.COUNT; c++)
                {
                    m_canMoves[c] = false;
                }
            }

            private bool[] m_canMoves;

            public bool canMove
            {
                get
                {
                    for (int c = 0; c < (int)CanMoveType.COUNT; c++)
                    {
                        if (m_canMoves[c])
                            return false;
                    }
                    return true;
                }
            }

            public void SetCanMove(bool value, CanMoveType type)
            {
                m_canMoves[(int)type] = !value;
            }
        }
    }
}
