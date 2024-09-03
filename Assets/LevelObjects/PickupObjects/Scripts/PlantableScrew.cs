///<summary>
/// Author: Halen
///
/// A Pickup that stops the player from moving and from their gravity from being changed.
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	using Player;

	namespace LevelObjects
	{
		namespace PickupObjects
		{
			public class PlantableScrew : PickupObject
			{
				private static PlayerController m_player;

                protected override void Start()
                {
                    base.Start();
					if (!m_player)
						m_player = GameManager.PlayerController;
                }

                public override void Use()
                {
					base.Use();

					Plant();
                }

				private void Plant()
				{

				}
            }
		}
	}
}
