///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace Millivolt
{
	namespace UI
	{
		public class TestMenu : UIMenu
		{
            public override void ActivateMenu()
            {
				Debug.Log("TIS ACTIVATED");
				isActive = true;
            }

			public override void DeactivateMenu()
			{
				Debug.Log("Tis no longer activated :(");
				isActive = false;
			}
        }
	}
}
