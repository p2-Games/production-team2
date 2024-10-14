///<summary>
/// Author: Halen
///
/// A Pickup that stops the player from moving and from their gravity from being changed.
///
///</summary>

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace PickupObjects
        {
            public class PlantableScrew : PickupObject
			{ 
				public override void Use()
                {
					base.Use();

					Plant();
                }

				private void Plant()
				{
					GameManager.PlayerController.SetCanMove(!m_inUse, Player.CanMoveType.HeldObject);
				}
            }
		}
	}
}
