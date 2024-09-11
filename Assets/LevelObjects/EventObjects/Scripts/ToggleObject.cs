///<summary>
/// Author: Halen
///
/// Interactable Object that can be toggled between an active and inactive state.
///
///</summary>

namespace Millivolt
{
    namespace LevelObjects
    {
        namespace EventObjects
        {
            public class ToggleObject : EventObject
            {
                public override void Interact()
                {
                    base.Interact();

                    isActive = !m_isActive;
                }
            }
        }
    }
}
