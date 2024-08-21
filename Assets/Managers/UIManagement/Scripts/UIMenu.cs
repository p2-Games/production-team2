///<summary>
/// Author: Emily
///
/// A base class for any menu in the game, all menus will derive from this
///
///</summary>

using UnityEditor;
using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public abstract class UIMenu : MonoBehaviour
		{
			public virtual void ActivateMenu()
			{

			}

            public virtual void DeactivateMenu()
            {

            }
        }
	}
}
