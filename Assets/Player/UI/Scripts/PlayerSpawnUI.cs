///<summary>
/// Author: Emily
///
/// The reverse of the death animation
///
///</summary>

using UnityEngine;
using UnityEngine.UI;
using Pixelplacement;

namespace Millivolt
{
    namespace UI
    {
        public class PlayerSpawnUI : ScreenFadeEffect
        {
            protected override void DisableThis()
            {
                base.DisableThis();
                GameManager.PlayerController.SetCanMove(true, Player.CanMoveType.Dead);
            }
        }
    }
}
